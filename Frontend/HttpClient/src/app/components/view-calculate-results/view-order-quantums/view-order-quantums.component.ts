import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnChanges } from '@angular/core';
import { Order } from '../../../models/order';
import { randomColor } from 'randomcolor';
import { forEach } from '@angular/router/src/utils/collection';
import { OrderQuantum } from '../../../models/order-quantum';

@Component({
    selector: 'sch-view-order-quantums',
    templateUrl: './view-order-quantums.component.html',
    styleUrls: ['./view-order-quantums.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewOrderQuantumsComponent implements OnInit, OnChanges {

    @Input()
    public order: Order;

    @Output()
    public selectedBlock: EventEmitter<{ orderQuantumIndex: number, blockIndex: number }>
        = new EventEmitter<{ orderQuantumIndex: number, blockIndex: number }>();

    public filteredOrderQuantums: OrderQuantum[] = [];

    public colors: any[] = [];

    constructor() { }

    ngOnInit() {
        this.defaultZoom();

        this.generateColors();
    }

    ngOnChanges() {
        this.defaultZoom();

        this.generateColors();
    }

    private generateColors() {
        this.order.orderQuantums.forEach(q => this.colors.push(randomColor({ luminosity: 'light' })));
    }

    public getMachiningStartTime(orderQuantumIndex: number, startTimeIndex: number) {
        return this.filteredOrderQuantums[orderQuantumIndex].machiningStartTimes[startTimeIndex];
    }

    public getAssemblingStartTime(orderQuantumIndex: number, startTimeIndex: number) {
        return this.filteredOrderQuantums[orderQuantumIndex].assemblingStartTimes[startTimeIndex];
    }

    public getBlockColor(orderQuantumIndex: number) {
        return this.colors[orderQuantumIndex];
    }

    public selectBlock(orderQuantumIndex: number, blockIndex: number) {
        this.selectedBlock.emit({ orderQuantumIndex: orderQuantumIndex, blockIndex: blockIndex });
    }

    public zoomPlus() {
        this.filteredOrderQuantums.forEach(oq => {
            for (let i = 0; i < oq.machiningDurations.length; i++) {
                oq.assemblingDurations[i] *= 1.1;
                oq.assemblingStartTimes[i] *= 1.1;
                oq.machiningDurations[i] *= 1.1;
                oq.machiningStartTimes[i] *= 1.1;
            }
        });
    }

    public zoomMinus() {
        this.filteredOrderQuantums.forEach(oq => {
            for (let i = 0; i < oq.machiningDurations.length; i++) {
                oq.assemblingDurations[i] *= 0.9;
                oq.assemblingStartTimes[i] *= 0.9;
                oq.machiningDurations[i] *= 0.9;
                oq.machiningStartTimes[i] *= 0.9;
            }
        });
    }

    public defaultZoom() {
        this.filteredOrderQuantums = [];
        this.order.orderQuantums.forEach(oq => this.filteredOrderQuantums.push({
            assemblingDurations: Object.assign(new Array<number>(), oq.assemblingDurations),
            assemblingStartTimes: Object.assign(new Array<number>(), oq.assemblingStartTimes),
            machiningDurations: Object.assign(new Array<number>(), oq.machiningDurations),
            machiningStartTimes: Object.assign(new Array<number>(), oq.machiningStartTimes),
            count: oq.count,
            itemsCountInOnePart: oq.itemsCountInOnePart
        }));
    }

}
