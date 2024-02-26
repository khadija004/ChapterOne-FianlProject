$(document).ready(function () {

    // Add active class to the current button (highlight it)
    var btnContainer = document.getElementById("myBtnContainer");
    var btns = btnContainer.getElementsByClassName("btn-tab");

    for (var i = 0; i < btns.length; i++) {
        btns[i].addEventListener("click", function () {
            var current = document.getElementsByClassName("active");
            current[0].className = current[0].className.replace(" active", "");
            this.className += " active";
        });
    }

    let additional = document.querySelector("#review-area .additional-information");
    let item = document.querySelector("#review-area .tab-menu ul .item2")
    item.addEventListener("click", function () {
        additional.classList.remove("d-none")
    })


    let description = document.querySelector("#review-area .additional-information");
    let item1 = document.querySelector("#review-area .tab-menu ul .item1")
    item1.addEventListener("click", function () {
        description.classList.add("d-none")
    })

    let reviews = document.querySelector("#review-area .user-comment");
    let item3 = document.querySelector("#review-area .tab-menu ul .item3")
    item3.addEventListener("click", function () {
        reviews.classList.remove("d-none")
    })

    let reviews1 = document.querySelector("#review-area .user-comment");
    let item4 = document.querySelector("#review-area .tab-menu ul .item1")
    item4.addEventListener("click", function () {
        reviews1.classList.add("d-none")
    })


})


//tab-menu
filterSelection("description")
function filterSelection(c) {
    var x, i;
    x = document.getElementsByClassName("filterDiv");
    console.log();
    if (c == "description") c = "";
    for (i = 0; i < x.length; i++) {
        w3RemoveClass(x[i], "show");
        if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show");
    }
}

function w3AddClass(element, name) {
    var i, arr1, arr2;
    arr1 = element.className.split(" ");
    arr2 = name.split(" ");
    for (i = 0; i < arr2.length; i++) {
        if (arr1.indexOf(arr2[i]) == -1) { element.className += " " + arr2[i]; }
    }
}

function w3RemoveClass(element, name) {
    var i, arr1, arr2;
    arr1 = element.className.split(" ");
    arr2 = name.split(" ");
    for (i = 0; i < arr2.length; i++) {
        while (arr1.indexOf(arr2[i]) > -1) {
            arr1.splice(arr1.indexOf(arr2[i]), 1);
        }
    }
    element.className = arr1.join(" ");
}