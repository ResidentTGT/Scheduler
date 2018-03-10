import { Component } from '@angular/core';
import { particlesParams } from '../../common/particles-params';

@Component({
    selector: 'sch-root',
    templateUrl: './root.component.html',
    styleUrls: ['./root.component.scss']
})
export class RootComponent {
    public particlesParams: object = particlesParams;
}
