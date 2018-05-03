var linha_clicada;
var tabela_clicada;
var id_clicado;
var valor_clicado;

$('.table').editableTableWidget();
$('table tbody td.td_valor, .save_0').on('change', function (evt, newValue) {
    var tb = $(this).closest('table');
    $.post("/Jquery/AtualizarValorMovimentacaoJquery", { id: id_modificado, novo_valor: newValue },
    function (resposta) {
        if (resposta != "0") {
            AtualizarMes(tb, id_modificado, newValue, resposta);
        }
    });
});

function AtualizarMes(tabela_mes, id_modificado, novo_valor, valor_original) {
    var diferenca = parseFloat(TrataValor(novo_valor) - TrataValor(valor_original));
    var valor_total = tabela_mes.find('.valor_total').html();
    tabela_mes.find('.valor_total').html((TrataValor(valor_total) + diferenca).toFixed(2).replace(".", ","));
    
    if (tabela_mes.find('.save_0').length) {
        var valor_sobra = tabela_mes.find('.valor_sobra').html();
        tabela_mes.find('.valor_sobra').html((TrataValor(valor_sobra) - diferenca).toFixed(2).replace(".", ","));
    }

    $.post("/Jquery/AtualizarValorSaveJquery", { id: id_modificado, dif: diferenca },
    function (resposta) {
        for (var i = 1; i <= 5; i++) {
            if (resposta[i] != '') {
                $(".save_" + i).html(resposta[i]);
            }
        }
        AtualizarSavings(0);
    });
}

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

function AtualizarStatusMovimentacao(id, status) {
    $.post("/Jquery/AtualizarStatusMovimentacao", { id: id, status: status },
    function (status_original) {
        if (status_original != status) {
            var novo_valor = "0";
            var valor_original = "0";
            switch (status) {
                case 'pago':
                    novo_valor = "0";
                    valor_original = valor_clicado;
                break;
                case 'normal':
                case 'definido':
                    if (status_original == 'pago') {
                        novo_valor = valor_clicado;
                        valor_original = "0";
                    }
                break;
            }
            AtualizarMes(tabela_clicada, id_clicado, novo_valor, valor_original);
        }
    });
}

function TrataValor(valor) {
    return parseFloat(valor.replace(",","."));
}

var menu = new BootstrapMenu('.table tbody tr', {
    fetchElementData: function(linha) {
        linha.find('td').each(function () {
            $(this).toggleClass('clicado');
        });
        linha_clicada = linha;
        tabela_clicada = linha_clicada.closest('table');
        id_clicado = linha_clicada.find('.id_movimentacao').val();
        valor_clicado = linha_clicada.find('td:eq(1)').html();
    },
    actions: [
        {
            name: 'Normal',
            onClick: function () {
                AtualizarStatusMovimentacao(linha_clicada.find('.id_movimentacao').val(), 'normal');
                linha_clicada.find('td').each(function () {
                    $(this).removeClass('td_definido');
                    $(this).removeClass('td_pago');
                    $(this).addClass('td_normal');
                });
            }
        },
        {
            name: 'Definido',
            onClick: function () {
                AtualizarStatusMovimentacao(linha_clicada.find('.id_movimentacao').val(), 'definido');
                linha_clicada.find('td').each(function () {
                    $(this).addClass('td_definido');
                    $(this).removeClass('td_pago');
                    $(this).removeClass('td_normal');
                });
            }
        },
        {
            name: 'Pago',
            onClick: function () {
                AtualizarStatusMovimentacao(linha_clicada.find('.id_movimentacao').val(), 'pago');
                linha_clicada.find('td').each(function () {
                    $(this).removeClass('td_definido');
                    $(this).addClass('td_pago');
                    $(this).removeClass('td_normal');
                });
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
            onClick: function () {
            }
        }
    ]
});

$(".navbar-brand").click(function() {
    $("#modal_consolidado").modal('show');
});

$(".div_movimentacoes").on('click','.tabela_mes thead',function() {
    $("#modal_movimentacao").modal('show');
});

$('#modal_consolidado').on('shown.bs.modal', function() {
    $(this).find('[autofocus]').focus();
});

$(".footer_form_consolidados").on("click", ".salvar", function () {
    var array = $("#form_consolidado").serializeArray();
    var json = {};
    
    jQuery.each(array, function () {
        json[this.name] = this.value || '';
    });

    $.post("/Jquery/AtualizarConsolidados", json,
    function (resposta) {
        $("#modal_consolidado").modal('hide');
    });
});

$(".footer_form_movimentacao").on("click", ".salvar", function () {
    var array = $("#form_movimentacao").serializeArray();
    var json = {};
    
    jQuery.each(array, function () {
        json[this.name] = this.value || '';
    });

    $.post("/Jquery/AtualizarMovimentacao", json,
    function (resposta) {
        $("#modal_movimentacao").modal('hide');
    });
});