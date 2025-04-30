let chart = null;

function getCssVar(name) {
    return getComputedStyle(document.documentElement).getPropertyValue(name).trim();
}

export function initializeChart(canvasRefOrId, data = [], labels = [], title = '') {
    let canvas = null;
    if (typeof canvasRefOrId === 'string') {
        canvas = document.getElementById(canvasRefOrId);
    } else if (canvasRefOrId instanceof HTMLCanvasElement) {
        canvas = canvasRefOrId;
    } else if (canvasRefOrId && canvasRefOrId.id) {
        canvas = document.getElementById(canvasRefOrId.id);
    }
    if (!canvas) return;
    const ctx = canvas.getContext('2d');
    if (!ctx) return;
    const bgColor = getCssVar('--bg-light') || '#fff';
    const textColor = getCssVar('--text-main') || '#222';
    const graphicColor = getCssVar('--accent-1') || '#222';

    if (chart) {
        chart.destroy();
        chart = null;
    }

    chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: title,
                data: data,
                borderColor: graphicColor,
                backgroundColor: textColor + '22',
                borderWidth: 2,
                pointRadius: 3,
                pointHoverRadius: 5,
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            animation: {
                duration: 2000,
                easing: 'easeOutQuart'
            },
            plugins: {
                legend: { display: false },
                title: { display: true, text: title, color: textColor },
                tooltip: {
                    mode: 'nearest',
                    intersect: true,
                    backgroundColor: bgColor,
                    titleColor: textColor,
                    bodyColor: textColor,
                    borderColor: textColor,
                    borderWidth: 1
                }
            },
            layout: { backgroundColor: bgColor },
            scales: {
                x: {
                    display: true,
                    title: { display: true, text: 'Date', color: textColor },
                    ticks: { color: textColor },
                    grid: { color: textColor + '22' }
                },
                y: {
                    display: true,
                    title: { display: true, text: 'Value', color: textColor },
                    ticks: { color: textColor },
                    grid: { color: textColor + '22' }
                }
            }
        }
    });
    canvas.style.background = bgColor;
    return true;
}

export function updateChart(data, labels, title) {
    if (chart) {
        chart.data.labels = labels;
        chart.data.datasets[0].data = data;
        chart.data.datasets[0].label = title;
        chart.options.plugins.title.text = title;
        chart.update();
        return true;
    }
    return false;
}

export function destroyChart() {
    if (chart) {
        chart.destroy();
        chart = null;
    }
} 