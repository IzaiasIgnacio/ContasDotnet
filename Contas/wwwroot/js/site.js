$('.table').editableTableWidget();
$('table tbody td.td_valor, .save_0').on('change', function (evt, newValue) {
    $.post("/Jquery/AtualizarValorMovimentacaoJquery", { id: id_modificado, novo_valor: newValue },
    function (resposta) {
        if (resposta != "0") {
            var diferenca = parseFloat(TrataValor(newValue) - TrataValor(resposta));
            /*$(".valor_total").each(function () {
                $(this).html((TrataValor($(this).html()) + diferenca).toFixed(2).replace(".", ","));
            });
            $(".valor_sobra").each(function () {
                $(this).html((TrataValor($(this).html()) - diferenca).toFixed(2).replace(".", ","));
            });*/

            $.post("/Jquery/AtualizarValorSaveJquery", { id: id_modificado, diferenca: diferenca },
            function (resposta) {
                for (var i = 1; i <= 5; i++) {
                    if (resposta[i] != '') {
                        $(".save_" + i).html(resposta[i]);
                    }
                }
                AtualizarSavings(0);
            });
        }
    });
});

function AtualizarSavings(c) {
    $.post("/Jquery/AtualizarTabelaSavings", { indice: c },
    function (resposta) {
        $(".tabela_saving_" + c).html(resposta);
        if (c < 5) {
            c++;
            AtualizarSavings(c);
        }
    });
}

function TrataValor(valor) {
    return parseFloat(valor.replace(",","."));
}

var menu = new BootstrapMenu('.table tbody tr', {
    actions: [
        {
            name: 'Normal',
            onClick: function () {
            }
        },
        {
            name: 'Definido',
            onClick: function () {
            }
        },
        {
            name: 'Pago',
            onClick: function () {
            }
        },
        {
            name: 'Editar',
            iconClass: 'fa-pencil',
            onClick: function () {
            }
        },
        {
            name: 'Excluir',
            //classNames: 'action-danger',
            onClick: function () {
            }
        }
    ]
});