
document.addEventListener("DOMContentLoaded", function() {
    // Função para obter o token JWT do localStorage
    const token = localStorage.getItem('jwt');

    const path = window.location.pathname;
    const pathParts = path.split('/');
    const userId = pathParts[2];  // O valor será '1234'
    console.log(userId);

    // Se o token não estiver presente, redirecione ou exiba uma mensagem de erro
    if (!token) {
        console.log('Usuário não autenticado. Redirecionando para o login...');
        window.location.href = '/login';  // Redirecionar para a página de login (ou outro comportamento)
        return;
    }

    // Fazer uma requisição GET para a API de Funcionários
    fetch('/api/User/v1/user/infos', {
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
                alert(`(API) Erro ao carregar os funcionários. Status ${response.status}`);
            });
        }
        return response.json(); // Caso a resposta seja bem-sucedida, converta para JSON
    })
    .then(data => {
        const boxHoursWorked = document.getElementById('boxHoursWorked');
        const boxDailyJourney = document.getElementById('boxDailyJourney');
        const boxWeeklyJourney = document.getElementById('boxWeeklyJourney');
        const container = document.getElementById('infoPunchClockRow');

        // Limpar o conteúdo anterior, se necessário
        boxHoursWorked.innerHTML = '';
        boxDailyJourney.innerHTML = '';
        boxWeeklyJourney.innerHTML = '';
        


       
        // Iterar sobre os funcionários e adicionar cada um ao corpo da tabela
        boxHoursWorked.innerHTML += `
            <span class="info-box-text text-center text-muted">Horas trabalhadas</span>
            <span class="info-box-number text-center text-muted mb-0" id="hoursWorked">${data.hoursWorked}h</span>
            `;

        boxDailyJourney.innerHTML += `
            <span class="info-box-text text-center text-muted">Jornada diária</span>
            <span class="info-box-number text-center text-muted mb-0">${data.settings.workdayHours}h</span>
            `;

        boxWeeklyJourney.innerHTML += `
            <span class="info-box-text text-center text-muted">Jornada Semanal</span>
            <span class="info-box-number text-center text-muted mb-0">${data.settings.weeklyJourney}h</span>
        `;

        if(data.registers.length == 0){
            const newHtml = `
                <div class="col-12" style="padding-top:0.5rem;">
                    <div class="info-box bg-light">
                        <div class="info-box-content" id="noRecentPunchs">
                            <span class="info-box-text text-left text-muted">Sem registros recentes</span>

                        </div>
                    </div>
                </div>
                `;
                container.innerHTML += newHtml;
        }

        data.registers.forEach(register => {
                            
            const checkIn = register.checkIns.length > 0 ? register.checkIns[0] : null;
            const checkOut = register.checkOuts.length > 0 ? register.checkOuts[0] : null; // Verifique se há checkout

            // Formatar data de check-in
            const checkInDate = checkIn ? new Date(checkIn.timestamp) : null;
            const checkOutDate = checkOut ? new Date(checkOut.timestamp) : null;

            const formatDate = (date) => {
            if (date) {
                const options = { weekday: 'long', year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false };
                return new Intl.DateTimeFormat('pt-BR', options).format(date);
            }
              return '';
            };

            if(checkIn != null && checkOut == null){
                 const htmlContent = `
                    <div class="col-12">                               
                        <div class="card">
                            <div class="card-body">
                                <div class="post">
                                    <div class="user-block">
                                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                                            ${data.name}
                                        </span>
                                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                                            ${register.checkIns[0].message} - ${checkInDate}
                                        </span>
                                    </div>
                                <p id="messagePunchClock">

                                </p>
                            </div>
                        </div>
                       </div>
                 `;

             container.innerHTML += htmlContent;
            }else if(checkIn != null && checkOut != null){
                const htmlContent = `
                    <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="post">
                                    <div class="user-block">
                                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                                            ${data.name}
                                        </span>
                                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                                            ${register.checkIns[0].message} - ${checkInDate}
                                        </span>
                                    </div>
                                <p id="messagePunchClock">

                                </p>
                            </div>
                        </div>
                       </div>
                 `;
                 container.innerHTML += htmlContent;

                 const newHtmlContent = `
                    <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="post">
                                    <div class="user-block">
                                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                                            ${data.name}
                                        </span>
                                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                                            ${register.checkOuts[0].message} - ${checkOutDate}
                                        </span>
                                    </div>
                                <p id="messagePunchClock">

                                </p>
                            </div>
                        </div>
                       </div>
                 `;
                 container.innerHTML += newHtmlContent;
            }
        
        });


        document.getElementById('dataUserName').textContent = data.name;
        document.getElementById('dataWeeklyJourney').textContent = data.hoursWorked;
        document.getElementById('dataRole').textContent = data.role;
        document.getElementById('dataDepartment').textContent = data.department;
        document.getElementById('dataPosition').textContent = data.position;
        document.getElementById('dataLevel').textContent = data.level;
        

    })
    .catch(error => {
        console.error('Erro ao buscar os dados dos funcionários:', error);
    });
});


