var dataTable;

$(document).ready(function () {
    cargarDatatable();

    // cuando cambien filtros, refresca
    $("#frmFiltros select").on("change", function () {
        dataTable.ajax.reload();
    });

    // si das click al botón Buscar, recarga sin recargar la página
    $("#frmFiltros").on("submit", function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });
});

function cargarDatatable() {
    dataTable = $("#TblContenido").DataTable({
        processing: true,
        serverSide: true,
        responsive: false,
        autoWidth: false,

        pageLength: 25,
        lengthMenu: [10, 25, 50, 100],

        ajax: {
            url: "/Panel/AtencionesRda/GetData",
            type: "POST",
            data: function (d) {
                // enviar filtros del formulario
                d.anioAtencion = $("#Filtros_AnioAtencion").val();
                d.donante = $("#Filtros_Donante").val();
                d.tipoPrestador = $("#Filtros_TipoPrestador").val();
                d.pais = $("#Filtros_Pais").val();
                d.grupoEtareo = $("#Filtros_GrupoEtareo").val();
                d.tipoConsulta = $("#Filtros_TipoConsulta").val();
            }
        },

        columns: [
            { data: "fechaAtencion" },
            { data: "donante" },
            { data: "pais" },
            { data: "nombrePrestador" },
            { data: "tipoPrestador" },
            { data: "grupoEtareo" },
            { data: "tipoConsulta" },
            { data: "identidadDisociada" },
            { data: "ilve" },
            { data: "telemedicina" },
        ],

        language: {
            decimal: "",
            emptyTable: "No hay registros",
            info: "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            infoEmpty: "Mostrando 0 a 0 de 0 Entradas",
            infoFiltered: "(Filtrado de _MAX_ total entradas)",
            lengthMenu: "Mostrar _MENU_ Entradas",
            loadingRecords: "Cargando...",
            processing: "Procesando...",
            search: "Buscar:",
            zeroRecords: "Sin resultados encontrados",
            paginate: {
                first: "Primero",
                last: "Ultimo",
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });
}
