using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace Contas.Services {
    public class PlanilhaService {
        SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum valueInputOption;
        SheetsService sheetsService;
        Spreadsheet planilhas;
        string idPlanilha;
        Double sobra;
        int maximoMovimentacoes;

        public PlanilhaService() {
            string[] Scopes = { SheetsService.Scope.Spreadsheets };

            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)) {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/contas.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            sheetsService = new SheetsService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                ApplicationName = "Google-SheetsSample/0.1",
            });

            idPlanilha = "1I6SEQnarqrTfe2uiyiaBgpxSdof8KE5DQaK4g7f15e4";

            maximoMovimentacoes = GetMaximoMovimentacoes();
        }

        public void AtualizarPlanilha() {
            SpreadsheetsResource.GetRequest get = sheetsService.Spreadsheets.Get(idPlanilha);
            planilhas = get.Execute();

            valueInputOption = (SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum)2;

            foreach (Sheet planilha in planilhas.Sheets) {
                ClearValuesRequest clearRequest = new ClearValuesRequest();
                SpreadsheetsResource.ValuesResource.ClearRequest request = sheetsService.Spreadsheets.Values.Clear(clearRequest, idPlanilha, "!A1:Z1000");
                request.Execute();

                for (int i=0;i<=5;i++) {
                    GerarTabelaMesSheet(i);
                }

                Layout();
            }
        }

        private void GerarTabelaMesSheet(int indice) {
            DateTime mes = DateTime.Now.AddMonths(indice);
            List<IList<object>> dados = new List<IList<object>>();
            
            var movimentacoes = ContasService.GetMovimentacoes(mes);
            Double saldo_parcial;
            if (indice == 0) {
                saldo_parcial = ConsolidadoService.GetValoresConsolidados();
            }
            else {
                saldo_parcial = sobra + Double.Parse(ConsolidadoService.GetValue("salario"));
            }

            var dt = mes.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR"));
            dados.Add(new List<object>() { dt.First().ToString().ToUpper()+ dt.Substring(1), saldo_parcial.ToString("F") });
            for (int i=0;i<maximoMovimentacoes;i++) {
                if (movimentacoes.ElementAtOrDefault(i) != null) {
                    dados.Add(new List<object>() { movimentacoes[i].Nome+ContasService.GetSiglaCartao(movimentacoes[i].IdCartao), movimentacoes[i].Valor.ToString("F") });
                }
                else {
                    dados.Add(new List<object>() { null });
                }
            }
            
            var save = (Double) ContasService.GetSaveMes(mes);
            dados.Add(new List<object>() { "Save", save.ToString("F") });

            var gastos = (Double) movimentacoes.Where(m => m.Tipo == "gasto" && m.Status != "pago").Sum(m => m.Valor);
            var rendas = (Double) movimentacoes.Where(m => m.Tipo == "renda").Sum(m => m.Valor);
            dados.Add(new List<object>() { "Total", (gastos - rendas).ToString("F") });

            sobra = saldo_parcial - gastos + rendas - save;
            dados.Add(new List<object>() { "Sobra", sobra.ToString("F") });

            char coluna = GetColumn(indice);
            char proxima = (char)(((int)coluna) + 1);
            string range = "!"+coluna+"2:"+proxima+dados.Count+1;

            ValueRange valueRange = new ValueRange();
            valueRange.Values = dados;

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, idPlanilha, range);
            updateRequest.ValueInputOption = valueInputOption;

            UpdateValuesResponse resposta = updateRequest.Execute();
        }

        private void Layout() {
            var limite = maximoMovimentacoes + 2;
            Request clearBoldRequest = new Request();
            clearBoldRequest.RepeatCell = new RepeatCellRequest();
            clearBoldRequest.RepeatCell.Fields = "userEnteredFormat(textFormat)";
            clearBoldRequest.RepeatCell.Range = new GridRange {
                SheetId = planilhas.Sheets[0].Properties.SheetId,
                StartColumnIndex = 1,
                EndColumnIndex = 18,
                StartRowIndex = 2,
                EndRowIndex = limite };
            clearBoldRequest.RepeatCell.Cell = new CellData { UserEnteredFormat = new CellFormat { TextFormat = new TextFormat { Bold = false } } };

            Request boldRequest = new Request();
            boldRequest.RepeatCell = new RepeatCellRequest();
            boldRequest.RepeatCell.Fields = "userEnteredFormat(textFormat)";
            boldRequest.RepeatCell.Range = new GridRange {
                SheetId = planilhas.Sheets[0].Properties.SheetId,
                StartColumnIndex = 1,
                EndColumnIndex = 18,
                StartRowIndex = limite,
                EndRowIndex = limite + 2 };
            boldRequest.RepeatCell.Cell = new CellData { UserEnteredFormat = new CellFormat { TextFormat = new TextFormat { Bold = true } } };
            
            BatchUpdateSpreadsheetRequest batch = new BatchUpdateSpreadsheetRequest();
            batch.Requests = new List<Request>();
            batch.Requests.Add(clearBoldRequest);
            batch.Requests.Add(boldRequest);

            SpreadsheetsResource.BatchUpdateRequest u = sheetsService.Spreadsheets.BatchUpdate(batch, idPlanilha);
            BatchUpdateSpreadsheetResponse responseResize = u.Execute();
        }
    
        private char GetColumn(int indice) {
            switch (indice) {
                case 1:
                    return 'E';
                case 2:
                    return 'H';
                case 3:
                    return 'K';
                case 4:
                    return 'N';
                case 5:
                    return 'Q';
            }
            return 'B';
        }

        private int GetMaximoMovimentacoes() {
            int maximo = 0;
            for (int i=0;i<=5;i++) {
                DateTime mes = DateTime.Now.AddMonths(i);
                int valor = ContasService.GetMovimentacoes(mes).Count;
                if (valor > maximo) {
                    maximo = valor;
                }
            }
            return maximo;
        }
        
    }
}