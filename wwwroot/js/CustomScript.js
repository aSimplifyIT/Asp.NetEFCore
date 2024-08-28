function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan-' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan-' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}