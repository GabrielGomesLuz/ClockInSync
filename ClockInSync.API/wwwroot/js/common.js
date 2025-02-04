export function convertToHtml(name, messageCheckIn, messageCheckOut, checkInDate, checkOutDate) {
    if (checkInDate != null && checkOutDate == null) {

        const htmlContent = `
                    <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="post">
                                    <div class="user-block">
                                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                                            ${name}
                                        </span>
                                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                                            ${messageCheckIn} - ${checkInDate}
                                        </span>
                                    </div>
                                <p id="messagePunchClock">

                                </p>
                            </div>
                        </div>
                       </div>
    `;
        return htmlContent;
    } else if (checkInDate != null && checkOutDate != null) {
        const htmlContent = `
                     <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="post">
                                    <div class="user-block">
                                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                                            ${name}
                                        </span>
                                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                                            ${messageCheckOut} - ${checkOutDate}
                                        </span>
                                    </div>
                                    <p id="messagePunchClock"></p>
                                </div>
                            </div>
                        </div>
                       </div>
                    <div class="col-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="post">
                                    <div class="user-block">
                                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                                            ${name}
                                        </span>
                                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                                            ${messageCheckIn} - ${checkInDate}
                                        </span>
                                    </div>
                                    <p id="messagePunchClock"></p>
                                </div>
                            </div>
                        </div>
                       </div>
    `;
        return htmlContent;
    } else {
        const htmlContent = `<div class="card">
            <div class="card-body">
                <div class="post">
                    <div class="user-block">
                        <span class="username" id="nameUserPunchClock" style="margin-left: 0;">
                            ${name}
                        </span>
                        <span class="description" id="datePunchClock" style="margin-left: 0;">
                            ${messageCheckOut} - ${checkOutDate}
                        </span>
                    </div>
                    <p id="messagePunchClock">

                    </p>
                </div>
            </div>
        </div>`;
        return htmlContent;
    }


}