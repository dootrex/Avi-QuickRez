

function gotClicked() {
    
    for (let i = 1; i < 10; i++) {
        console.log(document.querySelector(`.addTable${i}`).style.display )
        if (document.querySelector(`.addTable${i}`).style.display == 'none') {
            console.log("call", i)
            document.querySelector(`.addTable${i}`).style.display = 'table-row'
            break;
        }
    }

    


    
}