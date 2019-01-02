// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var dice = {
    roll: function (sides = 20) {
        var randomNumber = Math.floor(Math.random() * sides) + 1;
        return randomNumber;
    }
}



//Prints dice roll to the page

function printNumber(number) {
    var placeholder = document.getElementById('dieresult');
    placeholder.innerHTML = number;
}

var button = document.getElementById('rollbutton');

button.onclick = function () {
    var result = dice.roll();
    printNumber(result);
};

