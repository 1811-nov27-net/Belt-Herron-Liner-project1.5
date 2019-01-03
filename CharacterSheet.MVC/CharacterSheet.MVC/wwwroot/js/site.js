// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var dice = {
    roll: function (sides, bonus, numOfDice) {
        var total = bonus;
        for (var i = 0; i < numOfDice; i++) {
            total += Math.floor(Math.random() * sides) + 1;
        }
        return total;
    }
}

// rolls dice, prints to page.

function rollDice (sides = 20, bonus = 0, numOfDice = 1, message = 'Rolled') {
    var placeholder = document.getElementById('dieresult');
    var number = dice.roll(sides, bonus, numOfDice);
    placeholder.innerHTML = `${message} ${numOfDice}d${sides} + ${bonus}: ${number}`;
}

//Prints dice roll to the page

function updateHitpoints(ref = '') {
    var max = document.getElementById(ref + 'maxHP').value;
    var dmg = document.getElementById(ref + 'dmg').value;
    var disp = document.getElementById(ref + 'currentHP');
    disp.innerHTML = max-dmg;
}
