var oData = {};

fetch("https://randomuser.me/api/?inc=name,dob,phone,email,picture,login&noinfo&results=200")
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));

function _displayItems(data) {
    oData = data;
}


document.addEventListener('DOMContentLoaded', function () {

    var tokenKey = "accessToken";

    var popup = document.getElementById("popup");

    var popupBlack = document.getElementById("popup-black");

    var span = document.getElementById("close");

    //span.addEventListener('click', function () {
    //    popup.style.display = "none";
    //    popupBlack.style.display = "none";
    //});

    window.addEventListener('click', async (event) => {
        if (event.target.id == "close") {
            popup.style.display = "none";
            popupBlack.style.display = "none";
        }

        if (event.target.parentNode.className == "userInfo") {
            event.preventDefault();
            // получаем токен из sessionStorage
            const token = sessionStorage.getItem(tokenKey);
            // отправляем запрос к "/data
            const response = await fetch("/data", {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token  // передача токена в заголовке
                }
            });

            if (response.ok === true) {
                const data = await response.json();
                _displayUserInfo(event.target.parentNode.rowIndex);
                popup.style.display = "block";
                popupBlack.style.display = "block";
            }
            else {
                console.log("Status: ", response.status);
                alert("Для подробной информации необходимо войти в систему");
            }

        } else if (event.target == popupBlack) {
            popup.style.display = "none";
            popupBlack.style.display = "none";
        }

        if (event.target.id == "logOut") {
            e.preventDefault();
            document.getElementById("userName").innerText = "";
            document.getElementById("userInfo").style.display = "none";
            document.getElementById("loginForm").style.display = "block";
            sessionStorage.removeItem(tokenKey);
        }

    });


    // условный выход - просто удаляем токен и меняем видимость блоков
    //document.getElementById("logOut").addEventListener("click", e => {

    //    e.preventDefault();
    //    document.getElementById("userName").innerText = "";
    //    document.getElementById("userInfo").style.display = "none";
    //    document.getElementById("loginForm").style.display = "block";
    //    sessionStorage.removeItem(tokenKey);
    //});


});


function _formattDateOfBirth(date) {
    let dateOfBirth = date.slice(0, 10).split("-");
    let newDateOfBirth = new Date(dateOfBirth[0], dateOfBirth[1] - 1, dateOfBirth[2]).toDateString();
    return newDateOfBirth;
}

function _formattFullname(nameFirst, nameLast) {
    return nameFirst + " " + nameLast;
}

function _displayUserInfo(countRow) {

    const tBody = document.getElementById('info');
    tBody.innerHTML = '';

    let tr = tBody.insertRow();

    let td1 = tr.insertCell(0);
    let textNodeTd1 = document.createTextNode(_formattFullname(oData.results[countRow].name.first, oData.results[countRow].name.last));
    td1.appendChild(textNodeTd1);

    let td2 = tr.insertCell(1);
    let textNode = _formattDateOfBirth(oData.results[countRow].dob.date);
    td2.appendChild(document.createTextNode(textNode));

    let td3 = tr.insertCell(2);
    let img = document.createElement("IMG");
    img.src = oData.results[countRow].picture.thumbnail;
    td3.appendChild(img);

    let td4 = tr.insertCell(3);
    let textNode4 = document.createTextNode(oData.results[countRow].email);
    td4.appendChild(textNode4);

    let td5 = tr.insertCell(4);
    let textNode5 = document.createTextNode(oData.results[countRow].phone);
    td5.appendChild(textNode5);

}