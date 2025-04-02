import { Behaviour } from "@needle-tools/engine";
export class Rotate extends Behaviour {
    start(){
        console.log ("hey i'm here")
    }
    update(){
        this.gameObject.rotateY(this.context.time.deltaTime);
    }
}