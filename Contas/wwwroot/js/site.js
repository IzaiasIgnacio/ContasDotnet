// Write your JavaScript code.
$('.table').editableTableWidget();
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

$('table td').on('change', function (evt, newValue) {
    var td = $(this);
    $.post("/Jquery/AtualizarValorMovimentacaoJquery", { id: id_modificado, novo_valor: newValue },
    function (resposta) {
        td.append('<input type="hidden" class="id_movimentacao" value="'+id_modificado+'" />');
        var diferenca = parseFloat(TrataValor(newValue) - TrataValor(resposta));
        $(".valor_total").each(function () {
            $(this).html((TrataValor($(this).html()) + diferenca).toFixed(2).replace(".",","));
        });
        $(".valor_sobra").each(function () {
            $(this).html((TrataValor($(this).html()) - diferenca).toFixed(2).replace(".",","));
        });
    });
});

function TrataValor(valor) {
    return parseFloat(valor.replace(",","."));
}