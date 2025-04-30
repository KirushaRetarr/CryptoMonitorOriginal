window.downloadFile = function (base64Data, fileName, contentType) {
    const link = document.createElement('a');
    link.href = `data:${contentType};base64,${base64Data}`;
    link.download = fileName;
    link.click();
};

window.exportToPdf = function (data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();
    
    // Заголовок документа
    doc.setFontSize(16);
    doc.text('Crypto Assets Report', 14, 15);
    
    // Добавляем дату
    doc.setFontSize(10);
    doc.text(`Generated on: ${new Date().toLocaleString()}`, 14, 25);
    
    // Преобразуем данные в формат для таблицы
    const tableData = data.map(item => [
        item.symbol,
        item.name,
        item.latestPrice ? item.latestPrice.toFixed(2) : 'N/A',
        item.marketCap ? item.marketCap.toLocaleString() : 'N/A'
    ]);
    
    // Добавляем таблицу
    doc.autoTable({
        head: [['Symbol', 'Name', 'Price USD', 'Market Cap']],
        body: tableData,
        startY: 35,
        theme: 'grid',
        headStyles: {
            fillColor: [41, 128, 185],
            textColor: 255,
            fontSize: 10,
            fontStyle: 'bold'
        },
        styles: {
            fontSize: 9,
            cellPadding: 3
        },
        columnStyles: {
            0: { cellWidth: 30 },
            1: { cellWidth: 60 },
            2: { cellWidth: 40 },
            3: { cellWidth: 50 }
        }
    });
    
    // Сохраняем PDF
    doc.save(`crypto_assets_${new Date().toISOString().slice(0,10)}.pdf`);
}; 