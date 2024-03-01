$(document).ready(function () {
    //show-password
    const togglePassword1 = document.querySelector("#register-form .password-input .eyes");
    const password1 = document.querySelector("#register-form .password-input input");

    togglePassword1.addEventListener("click", function () {
        const type = password1.getAttribute("type") === "password" ? "text" : "password";
        password1.setAttribute("type", type);

        this.classList.toggle("bi-eye");
    });

    const form1 = document.querySelector("form");
    form1.addEventListener('submit', function (e) {
        e.preventDefault();
    });



})

$(document).ready(function(){
        //show-password
        const togglePassword2 = document.querySelector("#register-form .confirm-password-input .eyes");
        console.log(togglePassword2);
        const password2 = document.querySelector("#register-form .confirm-password-input input");
    
        togglePassword2.addEventListener("click", function () {
            const type = password2.getAttribute("type") === "password" ? "text" : "password";
            password2.setAttribute("type", type);
    
            this.classList.toggle("bi-eye");
        });
    
        const form2 = document.querySelector("form");
        form2.addEventListener('submit', function (e) {
            e.preventDefault();
        });
})