document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".btn-eliminar").forEach(btn => {

        btn.addEventListener("click", function () {

            const id = this.dataset.id;
            const url = this.dataset.url;

            Swal.fire({
                title: '¿Estás seguro?',
                text: "Este registro será eliminado",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar',
                confirmButtonColor: '#C63D96',
                cancelButtonColor: '#EE3A4F'
            }).then((result) => {

                if (result.isConfirmed) {
                    const form = document.getElementById("formEliminar");
                    document.getElementById("idEliminar").value = id;
                    form.action = url;
                    form.submit(); 
                }

            });
        });

    });

});
