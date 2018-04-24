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