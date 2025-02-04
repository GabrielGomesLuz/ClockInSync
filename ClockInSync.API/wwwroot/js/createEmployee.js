document.getElementById('postNewEmployee').addEventListener('submit', function(event) {
    event.preventDefault();  // Impede o comportamento padrão do form
    const token = localStorage.getItem('jwt');

    const inputName = document.getElementById('inputName');
    const inputEmail = document.getElementById('inputEmail');
    const inputPassword = document.getElementById('inputPassword');
    const inputRole = document.getElementById('inputRole');
    const inputDepartment =  document.getElementById('inputDepartment');
    const inputPosition = document.getElementById('inputPosition');
    const inputLevel = document.getElementById('inputLevel');
    const inputWorkdayHours = document.getElementById('inputDailyJourney');
    const inputWeeklyJourney = document.getElementById('inputWeeklyJourney');
    const inputOvertimeRate = document.getElementById('inputTaxOvertimeHours');

    fetch('/api/User/v1/register', {
        method: 'POST', 
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
            name: inputName,
            email: inputEmail,
            password: inputPassword,
            role: inputRole,
            department: inputDepartment,
            position: inputPosition,
            level: inputLevel,
            settings: {
                workdayHours: inputWorkdayHours,
                weeklyJourney: inputWeeklyJourney,
                overtimeRate: inputOvertimeRate
            }
        })
    })
    .then(response => response.json())
    .then(data => {
            Swal.fire({
    title: 'Sucesso!',
    text: 'Dados registrados com sucesso!',
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
