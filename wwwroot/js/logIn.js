
var tokenKey = "accessToken";

if (document.getElementById("submitLogin")) {
    document.getElementById("submitLogin").addEventListener("click", async (e) => {
        e.preventDefault();
        // отправляет запрос и получаем ответ
        const response = await fetch("/login", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                email: document.getElementById("email").value,
                password: document.getElementById("password").value
            })
        });
        // если запрос прошел нормально
        if (response.ok === true) {
            // получаем данные
            const data = await response.json();
            // изменяем содержимое и видимость блоков на странице\
            document.getElementById("loginForm").style.display = "none";
            alert("Успешная авторизация");
            document.getElementById("afterAut").style.display = "block";

            // сохраняем в хранилище sessionStorage токен доступа
            sessionStorage.setItem(tokenKey, data.access_token);
        }
        else {
            // если произошла ошибка, получаем код статуса
            console.log("Status: ", response.status);
            alert("Введите правильный логин и пароль");
        }
    });
        
}
