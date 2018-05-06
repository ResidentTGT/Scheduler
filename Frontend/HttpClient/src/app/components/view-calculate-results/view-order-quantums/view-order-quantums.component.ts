import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnChanges } from '@angular/core';
import { Order } from '../../../models/order';
import { randomColor } from 'randomcolor';
import { forEach } from '@angular/router/src/utils/collection';
import { OrderQuantum } from '../../../models/order-quantum';
import { OrderReport } from '../../../models/Reporting/OrderReport';
import { OrderBlock } from '../../../models/Reporting/OrderBlock';

@Component({
    selector: 'sch-view-order-quantums',
    templateUrl: './view-order-quantums.component.html',
    styleUrls: ['./view-order-quantums.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewOrderQuantumsComponent implements OnInit {

    @Input()
    public report: OrderReport;

    @Output()
    public selectedBlock: EventEmitter<OrderBlock> = new EventEmitter<OrderBlock>();

    public filteredBlocks: OrderBlock[] = [];

    public colors: { productionItemId: number, color: string }[] = [];

    constructor() { }

    ngOnInit() {
        this.defaultZoom();

        this.generateColors();
    }

    private generateColors() {
        const set = new Set<number>();
        this.report.orderBlocks.forEach(b => set.add(b.productionItemId));
        set.forEach(id => this.colors.push({ productionItemId: id, color: randomColor({ luminosity: 'light' }) }));
    }

    public getMachiningBlocks() {
        return this.filteredBlocks.filter(b => b.isMachining === true);
    }

    public getAssemblingBlocks() {
        return this.filteredBlocks.filter(b => b.isMachining !== true);
    }

    public getBlockColor(id: number) {
        return this.colors.filter(c => c.productionItemId === id)[0].color;
    }

    public selectBlock(orderBlock: OrderBlock) {
        debugger;
        this.selectedBlock.emit(Object.assign(new OrderBlock(), orderBlock));
    }

    public GetRealOrderBlock(id: number): OrderBlock {
        return this.report.orderBlocks.filter(b => b.id === id)[0];
    }

    public zoomPlus() {
        this.filteredBlocks.forEach(b => {
            b.duration *= 1.1;
            b.startTime *= 1.1;
        });
    }

    public zoomMinus() {
        this.filteredBlocks.forEach(b => {
            b.duration *= 0.9;
            b.startTime *= 0.9;
        });
    }

    public defaultZoom() {
        this.filteredBlocks = [];
        this.report.orderBlocks.forEach(b => this.filteredBlocks.push(Object.assign(new OrderBlock(), b)));
    }

}
