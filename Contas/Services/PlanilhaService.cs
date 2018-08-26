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
    public static class PlanilhaService {
        static SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum valueInputOption;
        static SheetsService sheetsService;
        static Spreadsheet planilhas;
        static string idPlanilha;
        static Double sobra;
        static int maximoMovimentacoes;
        static char colunaValorAnterior;
        static int linha_save;
        static int linha_total;
        static int linha_sobra;
        static int linha_savings;
        static int linha_total_savings;
        static int limite_savings;
        static int limite;

        static PlanilhaService() {
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
            limite = maximoMovimentacoes + 2;
            linha_save = maximoMovimentacoes + 3;
            linha_total = maximoMovimentacoes + 4;
            linha_sobra = maximoMovimentacoes + 5;
            linha_savings = maximoMovimentacoes + 7;
            limite_savings = maximoMovimentacoes + 8;
            linha_total_savings = maximoMovimentacoes + 9;
        }

        public static void AtualizarPlanilha() {
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

                for (int i=0;i<=5;i++) {
                    GerarTabelaSavingsSheet(i);
                }

                Layout();
            }
        }

        private static void GerarTabelaMesSheet(int indice) {
            DateTime mes = DateTime.Now.AddMonths(indice);
            List<IList<object>> dados = new List<IList<object>>();
            
            var movimentacoes = ContasService.GetMovimentacoes(mes).Where(m => m.Status != "pago").ToList();
            string saldo_mes;
            Double saldo_parcial;
            if (indice == 0) {
                saldo_mes = ConsolidadoService.GetValoresConsolidados().ToString("F");
            }
            else {
                saldo_mes = "=SUM("+Double.Parse(ConsolidadoService.GetValue("salario"))+"+"+colunaValorAnterior+linha_sobra+")";
            }
            saldo_parcial = sobra + Double.Parse(ConsolidadoService.GetValue("salario"));
            var dt = mes.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR"));
            dados.Add(new List<object>() { dt.First().ToString().ToUpper()+ dt.Substring(1), saldo_mes });
            for (int i=0;i<maximoMovimentacoes;i++) {
                if (movimentacoes.ElementAtOrDefault(i) != null) {
                    dados.Add(new List<object>() { "'"+movimentacoes[i].Nome+ContasService.GetSiglaCartao(movimentacoes[i].IdCartao), movimentacoes[i].Valor.ToString("F") });
                }
                else {
                    dados.Add(new List<object>() { null });
                }
            }
            
            var save = (Double) ContasService.GetSaveMes(mes);
            dados.Add(new List<object>() { "Save", save.ToString("F") });

            char coluna = GetColumn(indice);
            char proxima = (char)(((int)coluna) + 1);
            colunaValorAnterior = proxima;
            dados.Add(new List<object>() { "Total", "=SUM("+proxima+"3:"+proxima+limite+")" });

            var gastos = (Double) movimentacoes.Where(m => m.Tipo == "gasto" && m.Status != "pago").Sum(m => m.Valor);
            sobra = saldo_parcial - gastos - save;
            // dados.Add(new List<object>() { "Sobra", "="+proxima+"2-"+proxima+linha_total+"-"+proxima+linha_save });
            
            string range = "!"+coluna+"2:"+proxima+dados.Count+1;

            ValueRange valueRange = new ValueRange();
            valueRange.Values = dados;

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, idPlanilha, range);
            updateRequest.ValueInputOption = valueInputOption;

            UpdateValuesResponse resposta = updateRequest.Execute();
        }

        private static void GerarTabelaSavingsSheet(int indice) {
            DateTime mes = DateTime.Now.AddMonths(indice);
            List<IList<object>> dados = new List<IList<object>>();
            
            string saldo_mes;
            if (indice == 0) {
                saldo_mes = ConsolidadoService.GetValue("savings").Replace(".",",");
            }
            else {
                saldo_mes = "="+colunaValorAnterior+linha_total_savings;
            }
            var dt = mes.ToString("MMMM", CultureInfo.CreateSpecificCulture("pt-BR"));
            dados.Add(new List<object>() { dt.First().ToString().ToUpper()+ dt.Substring(1), saldo_mes });

            char coluna_nome = GetColumn(indice);
            char coluna_valor = (char)(((int)coluna_nome) + 1);
            colunaValorAnterior = coluna_valor;
            dados.Add(new List<object>() { "Save", "="+coluna_valor+linha_save });
            dados.Add(new List<object>() { "Total", "=SUM("+coluna_valor+linha_savings+":"+coluna_valor+limite_savings+")" });

            string range = "!"+coluna_nome+linha_savings+":"+coluna_valor+linha_total_savings;

            ValueRange valueRange = new ValueRange();
            valueRange.Values = dados;

            SpreadsheetsResource.ValuesResource.UpdateRequest updateRequest = sheetsService.Spreadsheets.Values.Update(valueRange, idPlanilha, range);
            updateRequest.ValueInputOption = valueInputOption;

            UpdateValuesResponse resposta = updateRequest.Execute();
        }

        private static void Layout() {
            Request clearBoldRequest = new Request();
            clearBoldRequest.RepeatCell = new RepeatCellRequest();
            clearBoldRequest.RepeatCell.Fields = "userEnteredFormat(textFormat)";
            clearBoldRequest.RepeatCell.Range = new GridRange {
                SheetId = planilhas.Sheets[0].Properties.SheetId,
                StartColumnIndex = 1,
                EndColumnIndex = 18,
                StartRowIndex = 3,
                EndRowIndex = linha_total_savings };
            clearBoldRequest.RepeatCell.Cell = new CellData { UserEnteredFormat = new CellFormat { TextFormat = new TextFormat { Bold = false } } };

            Request boldRequest = new Request();
            boldRequest.RepeatCell = new RepeatCellRequest();
            boldRequest.RepeatCell.Fields = "userEnteredFormat(textFormat)";
            boldRequest.RepeatCell.Range = new GridRange {
                SheetId = planilhas.Sheets[0].Properties.SheetId,
                StartColumnIndex = 1,
                EndColumnIndex = 18,
                StartRowIndex = limite,
                EndRowIndex = linha_total_savings };
            boldRequest.RepeatCell.Cell = new CellData { UserEnteredFormat = new CellFormat { TextFormat = new TextFormat { Bold = true } } };
            
            BatchUpdateSpreadsheetRequest batch = new BatchUpdateSpreadsheetRequest();
            batch.Requests = new List<Request>();
            batch.Requests.Add(clearBoldRequest);
            batch.Requests.Add(boldRequest);

            SpreadsheetsResource.BatchUpdateRequest u = sheetsService.Spreadsheets.BatchUpdate(batch, idPlanilha);
            BatchUpdateSpreadsheetResponse responseResize = u.Execute();
        }
    
        private static char GetColumn(int indice) {
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

        private static int GetMaximoMovimentacoes() {
            int maximo = 0;
            for (int i=0;i<=5;i++) {
                DateTime mes = DateTime.Now.AddMonths(i);
                int valor = ContasService.GetMovimentacoes(mes).Where(m => m.Tipo == "gasto" && m.Status != "pago").Count();
                if (valor > maximo) {
                    maximo = valor;
                }
            }
            return maximo;
        }
        
    }
}