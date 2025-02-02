document.getElementById('registerClock').addEventListener('click', async (event) => {
    event.preventDefault();  // Impede o comportamento padrão do form
    const token = localStorage.getItem('jwt');


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

