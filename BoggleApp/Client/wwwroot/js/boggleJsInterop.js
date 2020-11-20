window.initializeInput = () => {
    var input = document.getElementById("boggleWordInput");
    input.addEventListener("keydown", function (evt) {
        if (window.boggleBoard.indexOf(evt.key) == -1 && (evt.which != 13 && evt.which != 32 && evt.which != 8))
                evt.preventDefault();
    });
}


window.boggleBoard = [];


window.setBoggleBoard = (boggleBoardValues) => {
    window.boggleBoard = boggleBoardValues;
}


window.clearBoggleWordInput = () => {
    var input = document.getElementById("boggleWordInput");
    input.value = "";
}


