import {
    Behaviour,
    serializable,
    PointerEventData,
    IPointerEventHandler
} from "@needle-tools/engine";

export class DragRotate extends Behaviour implements IPointerEventHandler {
    @serializable()
    rotationSpeed: number = 0.5;

    private isDragging: boolean = false;
    private lastPointerX: number = 0;

    onPointerDown(args: PointerEventData) {
        this.isDragging = true;
        this.lastPointerX = (args.event as PointerEvent).clientX;

        // Hindra OrbitControls från att reagera
        args.event.stopImmediatePropagation();

        // Lägg till globala listeners så vi kan blockera kamerarörelser helt
        window.addEventListener("pointermove", this.onGlobalPointerMove, true);
        window.addEventListener("pointerup", this.onGlobalPointerUp, true);
    }

    private onGlobalPointerMove = (event: PointerEvent) => {
        if (!this.isDragging) return;

        const currentX = event.clientX;
        const deltaX = currentX - this.lastPointerX;
        this.lastPointerX = currentX;

        this.gameObject.rotateY(deltaX * this.rotationSpeed * 0.016); // ungefärligt deltaTime

        // Stoppa kameran från att få eventet
        event.stopImmediatePropagation();
    };

    private onGlobalPointerUp = (event: PointerEvent) => {
        this.isDragging = false;

        window.removeEventListener("pointermove", this.onGlobalPointerMove, true);
        window.removeEventListener("pointerup", this.onGlobalPointerUp, true);

        event.stopImmediatePropagation();
    };
}