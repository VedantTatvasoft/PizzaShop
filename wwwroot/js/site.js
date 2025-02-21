// DrowpDown code 
document.getElementById("countryDropdown").addEventListener("change", function () {
    var countryId = this.value;
    fetch(`/Profile/GetStates?countryId=${countryId}`)
        .then(response => response.json())
        .then(data => {
            var stateDropdown = document.getElementById("stateDropdown");
            console.log("data :-- ", data);
            stateDropdown.innerHTML = '<option value="">-- Select state --</option>';
            document.getElementById("cityDropdown").innerHTML = '<option value="">-- Select City --</option>';
            data.forEach(state => {
                stateDropdown.innerHTML += `<option value="${state.stateid}">${state.name}</option>`;
            });
        });


});

document.getElementById("stateDropdown").addEventListener("change", function () {
    var stateId = this.value;
    console.log("")
    fetch(`/Profile/GetCities?stateId=${stateId}`)
        .then(response => response.json())
        .then(data => {
            var cityDropdown = document.getElementById("cityDropdown");
            cityDropdown.innerHTML = '<option value="">-- Select City --</option>';
            data.forEach(city => {
                cityDropdown.innerHTML += `<option value="${city.cityid}">${city.name}</option>`;
            });
        });
});



const currenttogglePassword = document.querySelector('#currenttogglePassword');
const currentPassword = document.querySelector('#currentPassword');
currenttogglePassword.addEventListener('click', (event) => {
    const type = currentPassword.getAttribute('type') === 'password' ? 'text' : 'password';
    currentPassword.setAttribute('type', type);
    if (type === "password") {
        event.target.classList.add("bi-eye-slash");
        event.target.classList.remove("bi-eye")
    }
    else {
        event.target.classList.remove("bi-eye-slash");
        event.target.classList.add("bi-eye")

    }
});
const newtogglePassword = document.querySelector('#newtogglePassword');
const newPassword = document.querySelector('#newPassword');
newtogglePassword.addEventListener('click', (event) => {
    const type = newPassword.getAttribute('type') === 'password' ? 'text' : 'password';
    newPassword.setAttribute('type', type);
    if (type === "password") {
        event.target.classList.add("bi-eye-slash");
        event.target.classList.remove("bi-eye")
    }
    else {
        event.target.classList.remove("bi-eye-slash");
        event.target.classList.add("bi-eye")

    }
});
const cnewtogglePassword = document.querySelector('#cnewtogglePassword');
const cnewPassword = document.querySelector('#cnewPassword');
cnewtogglePassword.addEventListener('click', (event) => {
    const type = cnewPassword.getAttribute('type') === 'password' ? 'text' : 'password';
    cnewPassword.setAttribute('type', type);
    if (type === "password") {
        event.target.classList.add("bi-eye-slash");
        event.target.classList.remove("bi-eye")
    }
    else {
        event.target.classList.remove("bi-eye-slash");
        event.target.classList.add("bi-eye")

    }
});