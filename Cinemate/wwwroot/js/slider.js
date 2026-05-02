const cards = document.querySelectorAll(".film-karti");
let currentIndex = 2; // ortadaki kart

document.querySelector(".sag-ok").addEventListener("click", () => {
    currentIndex = (currentIndex + 1) % cards.length;
    updateCards();
});

document.querySelector(".sol-ok").addEventListener("click", () => {
    currentIndex = (currentIndex - 1 + cards.length) % cards.length;
    updateCards();
});

function updateCards() {
    cards.forEach((card, index) => {
        card.classList.remove("orta-kart", "gizli-kart-1", "gizli-kart-2");

        let diff = index - currentIndex;

        if (diff === 0) card.classList.add("orta-kart");
        else if (Math.abs(diff) === 1) card.classList.add("gizli-kart-2");
        else card.classList.add("gizli-kart-1");
    });
}
