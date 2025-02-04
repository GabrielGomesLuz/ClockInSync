// Função para fazer login
function login(event) {
    event.preventDefault();  // Impede o envio padrão do formulário

    // Coletando os dados do formulário
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const isValid = validateLogin(email,password);

    if(!isValid){
        return;
    }

    // Configuração da requisição POST com fetch
    fetch('/api/User/v1/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json', // Indicando que os dados são no formato JSON
        },
        body: JSON.stringify({ email, password }) // Envia os dados no corpo da requisição
    })
    .then(response => {
    if (!response.ok) {
        // Tenta capturar a resposta de erro da API
        return response.json().then(errorData => {
            // Rejeita a Promise com a mensagem de erro retornada pela API
            console.log(errorData);
            return Promise.reject(errorData.message || 'Erro desconhecido');
        });
    }
    return response.json(); // Se for bem-sucedida, converte a resposta em JSON
})
.then(data => {
    // Armazenar o JWT no localStorage
    localStorage.setItem('jwt', data.jwtToken);

    // Atualizar o status de login
    document.getElementById('login-status').textContent = data.message;
    document.getElementById('login-status').style.color = 'green';
     window.location.href = '/funcionarios';
})
.catch(error => {
        // Exibe uma mensagem de erro
        document.getElementById('login-status').textContent = error;
        document.getElementById('login-status').style.color = 'red';
    });
}

function validateLogin(email,password){

    const login = document.getElementById('login-status');

    if(email === '' || password === ''){

        login.textContent = 'Todos os campos precisam ser preenchidos.';
        login.style.color = 'red';
        return false;
    }
    return true;

}



// Adiciona o evento ao formulário para chamar a função de login
document.getElementById('login-form').addEventListener('submit', login);