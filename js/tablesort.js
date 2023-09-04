document$.subscribe(function() {
    var tables = document.querySelectorAll("article table:not([class])")
    tables.forEach(function(table) {
        var th = table.querySelector("th");
        if (th !== null) {
            th.setAttribute("data-sort-default", "");
        }
        new Tablesort(table);
    })
  })