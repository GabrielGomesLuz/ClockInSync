const token = localStorage.getItem('jwt');
import { convertToHtml } from "./common.js";

document.getElementById('registerClock').addEventListener('click', async (event) => {
    event.preventDefault();  // Impede o comportamento padrão do form
    


    try {
        const response = await fetch('/api/PunchClock/v1/punch-clock', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            Swal.fire({
                title: 'Erro!',
                text: 'Ocorreu um erro ao registrar seu ponto!',
                icon: 'error',
                confirmButtonText: 'Ok'
            })
        }

        Swal.fire({
            title: 'Sucesso!',
            text: 'Ponto registrado com sucesso!',
            icon: 'success',
            confirmButtonText: 'Ok'
        })
    } catch (error) {

        Swal.fire({
            title: 'Erro!',
            text: 'Ocorreu um erro ao registrar seu ponto!',
            icon: 'error',
            confirmButtonText: 'Ok'
        })
        console.log(error)
    }


});

const responseInfoUser = await fetch('/api/User/v1/user/info', {
    method: 'GET',
    headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${token}`
    }
});

if (!responseInfoUser.ok) {

    Swal.fire({
        title: 'Erro!',
        text: 'Ocorreu um erro ao buscar informações do usuário!',
        icon: 'error',
        confirmButtonText: 'Ok',
    })
}

const userData = await responseInfoUser.json();
console.log(userData);

const container = document.getElementById('infoPunchClockRow');
const userName = document.getElementById('employeeName');
userName.textContent = userData.name;
const userWeeklyJourney = document.getElementById('weeklyJourney');
userWeeklyJourney.textContent = userData.settings.weeklyJourney + 'h';
const userDailyJourney = document.getElementById('dailyJourney');
userDailyJourney.textContent = userData.settings.workdayHours + 'h';
const userPosition = document.getElementById('employeePosition');
userPosition.textContent = userData.position;
//TODO implementar metodo no backEnd para realizar calculo da jornada mensal, considerando finais de semana e feriados.

const limit = 8;
const response = await fetch(`/api/PunchClock/v1/history?limit=${limit}`, {
    method: 'GET',
    headers: {
        'Content-type': 'application/json',
        'Authorization': `Bearer ${token}`
    }
});

if (!response.ok) {

    container.innerHTML += `
                <div class="col-12" style="padding-top:0.5rem;">
                    <div class="info-box bg-light">
                        <div class="info-box-content" id="errorSearchingPunchs">
                            <span class="info-box-text text-left text-muted">Ocorreu um erro ao buscar registros recentes.</span>

                        </div>
                    </div>
                </div>
                `;
}

const data = await response.json();
console.log(data);
data.registers.forEach((register) => {

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
        return null;
    };
    console.log(formatDate(checkOutDate))
    const formattedCheckInDate = formatDate(checkInDate);
    const formatCheckoutDate = formatDate(checkOutDate);

    if (checkIn != null && checkOut == null && formattedCheckInDate != null) {
        container.innerHTML += convertToHtml(userData.name, register.checkIns[0].message, null, formattedCheckInDate, null);
    }
    else if (checkIn != null && checkOut != null && formattedCheckInDate != null && formatCheckoutDate != null) {
        container.innerHTML += convertToHtml(userData.name, register.checkIns[0].message, register.checkOuts[0].message, formattedCheckInDate, formatCheckoutDate);
    }
})



