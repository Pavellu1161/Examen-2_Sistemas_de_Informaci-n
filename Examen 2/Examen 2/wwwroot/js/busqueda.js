function homework_search() {
    const search = document.getElementById("txt-searchTarea").value;
    const currentURL = window.location.href.substring(
        window.location.href.lastIndexOf('/') + 1
    ).split("?")[0];    

    window.location = `${currentURL}?search=${search}`;
}

function category_search() {
    const search = document.getElementById("txt-searchcategory").value;
    const currentURL = window.location.href.substring(
        window.location.href.lastIndexOf('/') + 1
    ).split("?")[0];

    window.location = `${currentURL}?search=${search}`;
}