document.addEventListener("DOMContentLoaded", function() {
    // Função para obter o token JWT do localStorage
    const token = localStorage.getItem('jwt');

    const path = window.location.pathname;
    const pathParts = path.split('/');
    const userId = pathParts[3];  // O valor será '1234'
    console.log(userId);

    // Se o token não estiver presente, redirecione ou exiba uma mensagem de erro
    if (!token) {
        console.log('Usuário não autenticado. Redirecionando para o login...');
        window.location.href = '/login';  // Redirecionar para a página de login (ou outro comportamento)
        return;
    }

    // Fazer uma requisição GET para a API de Funcionários
    fetch('/api/User/v1/user/edit/infos', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
            'userId': userId // Incluir o userId no cabeçalho
        }
    })
    .then(response => {
        if (!response.ok) {
            // Se a resposta não for ok (erro), você pode lidar com isso de acordo
            return response.json().then(errorData => {
                console.error('Erro:', errorData);
                alert('Erro ao carregar os funcionários.');
            });
        }
        return response.json(); // Caso a resposta seja bem-sucedida, converta para JSON
    })
    .then(data => {
        
        const inputName = document.getElementById('inputName').value = data.name;
        const inputEmail = document.getElementById('inputEmail').value = data.email;
        const inputRole = document.getElementById('inputRole').value = data.role;
        const inputDepartment = document.getElementById('inputDepartment').value = data.department;
        const inputPosition = document.getElementById('inputPosition').value = data.position;
        const inputLevel = document.getElementById('inputLevel').value = data.level;
        

        const inputDailyJourney = document.getElementById('inputDailyJourney').value = data.settings.workdayHours;
        const inputWeeklyJourney = document.getElementById('inputWeeklyJourney').value = data.settings.weeklyJourney;
        const inputTaxOvertimeHours = document.getElementById('inputTaxOvertimeHours').value = data.settings.overtimeRate;

    })
    .catch(error => {
        console.error('Erro ao buscar os dados dos funcionários:', error);
    });
});


document.getElementById('postEdit').addEventListener('submit', function(event) {
    event.preventDefault();  // Impede o comportamento padrão do form
    const token = localStorage.getItem('jwt');
    const path = window.location.pathname;
    const pathParts = path.split('/');
    const userId = pathParts[3];
    const formData = new FormData(this);  // Obtém os dados do formulário

    fetch('/api/User/v1/put/user/edit', {
        method: 'PUT',  // Usando PUT para atualizar o recurso
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
            id: userId,
            name: document.getElementById('inputName').value,
            email: document.getElementById('inputEmail').value,
            role: document.getElementById('inputRole').value,
            department: document.getElementById('inputDepartment').value,
            position: document.getElementById('inputPosition').value,
            level: document.getElementById('inputLevel').value,
            settings: {
                workdayHours: document.getElementById('inputDailyJourney').value,
                weeklyJourney: document.getElementById('inputWeeklyJourney').value,
                overtimeRate: document.getElementById('inputTaxOvertimeHours').value
            }
        })
    })
    .then(response => response.json())
    .then(data => {
            Swal.fire({
    title: 'Sucesso!',
    text: 'Dados atualizados com sucesso!',
    icon: 'success',
    confirmButtonText: 'Ok'
}).then(() => {
    window.location.href = '/funcionarios'; // Redirecionamento após o sucesso
});
    })
    .catch(error => {
        console.error('Error:', error);
    });
});  // Fechamento do evento

