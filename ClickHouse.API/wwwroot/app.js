function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function formatDate(dateString) {
    const date = new Date(dateString);
    if (date.valueOf() === 0) {
        return '-';
    }

    return date.toLocaleString();
}

let fetchingStats = false;
async function fetchAndUpdateStats() {
    if (fetchingStats) {
        return;
    }

    fetchingStats = true;
    try {
        const response = await fetch('/api/stats');
        const data = await response.json();

        document.getElementById('latestSensorTime').textContent = formatDate(data.latestSensorTime);
        document.getElementById('totalSensors').textContent = formatNumber(data.totalSensors);
        document.getElementById('totalSamples').textContent = formatNumber(data.totalSamples);
    } catch (error) {
        console.error('Error fetching stats:', error);
    } finally {
        fetchingStats = false;
    }
}


let fetchingListStats = false;
async function fetchAndUpdateListStats() {
    if (fetchingListStats) {
        return;
    }

    fetchingListStats = true;
    try {
        const response = await fetch('/api/list-stats');
        const data = await response.json();

        updateSamplesPerDateTable(data.samplesPerDate);
        updateSamplesPerSensorTable(data.samplesPerSensor);
    } catch (error) {
        console.error('Error fetching list stats:', error);
    } finally {
        fetchingListStats = false;
    }
}

const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
function updateSamplesPerDateTable(data) {
    let html = '';
    data.forEach(item => {
        const itemDate = new Date(item.date);
        html += `
            <tr>
                <td>${monthNames[itemDate.getUTCMonth()]} ${itemDate.getUTCFullYear()}</td>
                <td class="text-right count-value">${formatNumber(item.count)}</td>
            </tr>
        `;
    });

    document.getElementById('samplesPerDateTable').innerHTML = html;
}

function updateSamplesPerSensorTable(data) {
    let html = '';
    data.forEach(item => {
        html += `
            <tr>
                <td>${item.sensorType}</td>
                <td class="text-right count-value">${formatNumber(item.count)}</td>
            </tr>
        `;
    });

    document.getElementById('samplesPerSensorTable').innerHTML = html;
}

document.addEventListener('DOMContentLoaded', () => {
    fetchAndUpdateStats();
    fetchAndUpdateListStats();

    setInterval(fetchAndUpdateStats, 250);
    setInterval(fetchAndUpdateListStats, 1000);
});
