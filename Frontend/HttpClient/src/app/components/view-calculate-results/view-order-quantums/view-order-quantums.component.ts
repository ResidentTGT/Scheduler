import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { Order } from '../../../models/order';

@Component({
    selector: 'sch-view-order-quantums',
    templateUrl: './view-order-quantums.component.html',
    styleUrls: ['./view-order-quantums.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewOrderQuantumsComponent implements OnInit {

    @Input()
    public order: Order;

    constructor() { }

    ngOnInit() {

    }

    public getMachiningStartTime(orderQuantumIndex: number, startTimeIndex: number) {
        return this.order.orderQuantums[orderQuantumIndex].machiningStartTimes[startTimeIndex];
    }

    public getAssemblingStartTime(orderQuantumIndex: number, startTimeIndex: number) {
        return this.order.orderQuantums[orderQuantumIndex].assemblingStartTimes[startTimeIndex];
    }

    public getBlockColor(orderQuantumIndex: number) {

    }

}
