// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

  
 /*    var filtroInput = document.getElementById("filtroInput");

    filtroInput.addEventListener("input", filtrarOpciones);



    function filtrarOpciones(){

        var filtro = filtroInput.value.toLowerCase();
        var select = document.querySelector("tbody");


        
        
        var opciones = select.getElementsByTagName("tr");
        for (var i = 0; i < opciones.length; i++) {
            var opcionTexto = opciones[i].innerHTML.toLowerCase();
           if (opcionTexto.includes(filtro)) {
                opciones[i].style.display = "";
            }
            else {
                opciones[i].style.display = "none";
            }
        }
    }







    var filtrochecks = document.getElementById("porDisponibilidad");

    filtrochecks.addEventListener("change", filtrarDisponibilidad);



    function filtrarDisponibilidad(){

       

        console.log(filtrochecks.checked);

        var select = document.querySelector("tbody");


        
        
        var opciones = select.getElementsByTagName("tr");
        for (var i = 0; i < opciones.length; i++) {

            console.log(opciones[i]);
            var checkbox = opciones[i].querySelector('input[type="checkbox"]');
    
            console.log(filtrochecks.checked, checkbox.checked);
            if (filtrochecks.checked && !checkbox.checked) {
                opciones[i].style.display = "none";
            } else {
                opciones[i].style.display = "table-row";
            }
        }
    }
 */


    var filtroInput = document.getElementById("filtroInput");
    var filtrochecks = document.getElementById("porDisponibilidad");
    
    filtroInput.addEventListener("input", filtrar);
    
    if (filtrochecks) {
        filtrochecks.addEventListener("change", filtrar);
    }
    
    function filtrar() {
        var filtro = filtroInput.value.toLowerCase();
        var filtroDisponibilidad = filtrochecks ? filtrochecks.checked : false;
    
        var select = document.querySelector("tbody");
        var opciones = select.getElementsByTagName("tr");
    
        for (var i = 0; i < opciones.length; i++) {
            var opcionTexto = opciones[i].innerHTML.toLowerCase();
            var checkbox = opciones[i].querySelector('input[type="checkbox"]');
            var checkboxChecked = checkbox ? checkbox.checked : false;
    
            if ((opcionTexto.includes(filtro) && (!filtroDisponibilidad || checkboxChecked)) ||
                (!filtro && !filtroDisponibilidad)) {
                opciones[i].style.display = "table-row";
            } else {
                opciones[i].style.display = "none";
            }
        }
    }