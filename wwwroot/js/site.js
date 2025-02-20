// DrowpDown code 
document.getElementById("countryDropdown").addEventListener("change", function () {
    var countryId = this.value;
    fetch(`/Profile/GetStates?countryId=${ countryId }`)
        .then(response => response.json())
        .then(data => {
            var stateDropdown = document.getElementById("stateDropdown");
            console.log("data :-- ",data);
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
    fetch(`/Profile/GetCities?stateId=${ stateId }`)
        .then(response => response.json())
        .then(data => {
            var cityDropdown = document.getElementById("cityDropdown");
            cityDropdown.innerHTML = '<option value="">-- Select City --</option>';
            data.forEach(city => {
                cityDropdown.innerHTML += `<option value="${city.cityid}">${city.name}</option>`;
            });
        });
});