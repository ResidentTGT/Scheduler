import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnChanges } from '@angular/core';
import { Order } from '../../../models/order';
import { ProductionItem } from '../../../models/production-item';
import { randomColor } from 'randomcolor';
import { ProductionItemQuantumsGroup } from '../../../models/production-item-quantums-group';

@Component({
    selector: 'sch-view-production-item-quantums-groups',
    templateUrl: './view-production-item-quantums-groups.component.html',
    styleUrls: ['./view-production-item-quantums-groups.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewProductionItemQuantumsGroupsComponent implements OnInit, OnChanges {

    @Input()
    public order: Order;

    @Input()
    public selectedBlock: { orderQuantumIndex: number, offset: number };

    @Output()
    selectedGroup: EventEmitter<{ orderQuantumIndex: number, groupIndex: number, offset: number }>
        = new EventEmitter<{ orderQuantumIndex: number, groupIndex: number, offset: number }>();

    public productionItem: ProductionItem;
    public colors: any[] = [];
    public allWorkshops: Set<number> = new Set<number>();

    constructor() { }

    ngOnInit() {
        this.defaultZoom();

        this.generateColors();
        this.getAllWorkshops();
    }

    ngOnChanges() {
        this.defaultZoom();

        this.generateColors();
        this.getAllWorkshops();
    }

    private generateColors() {
        this.productionItem.productionItemQuantumsGroups.forEach(q => this.colors.push(randomColor({ luminosity: 'light' })));
    }

    public defaultZoom() {
        this.productionItem = new ProductionItem();
        this.productionItem.productionItemQuantumsGroups = new Array<ProductionItemQuantumsGroup>();
        this.order.orderQuantums[this.selectedBlock.orderQuantumIndex].productionItem.productionItemQuantumsGroups
            .forEach(oq => this.productionItem.productionItemQuantumsGroups.push({
                workshopSequence: Object.assign(new Array<number>(), oq.workshopSequence),
                workshopStartTimes: Object.assign(new Array<number>(), oq.workshopStartTimes),
                workshopEndTimes: Object.assign(new Array<number>(), oq.workshopEndTimes),
                workshopDurations: Object.assign(new Array<number>(), oq.workshopDurations),
            }));
    }

    public zoomPlus() {
        this.productionItem.productionItemQuantumsGroups.forEach(g => {
            for (let i = 0; i < g.workshopDurations.length; i++) {
                g.workshopDurations[i] *= 1.1;
                g.workshopStartTimes[i] *= 1.1;
            }
        });
    }

    public zoomMinus() {
        this.productionItem.productionItemQuantumsGroups.forEach(g => {
            for (let i = 0; i < g.workshopDurations.length; i++) {
                g.workshopDurations[i] *= 0.9;
                g.workshopStartTimes[i] *= 0.9;
            }
        });
    }

    public getAllWorkshops() {
        this.allWorkshops = new Set<number>();
        this.productionItem.productionItemQuantumsGroups.forEach(g => g.workshopSequence.forEach(s => this.allWorkshops.add(s)));
    }


    public isGroupInWorkshop(workshopId: number, groupIndex: number) {
        return this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.some(s => s === workshopId);
    }

    //#region Получение времен для рисования блоков
    public getWorkshopDuration(workshopId: number, groupIndex: number) {
        const workshopIndex = this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.indexOf(workshopId);
        return this.productionItem.productionItemQuantumsGroups[groupIndex].workshopDurations[workshopIndex];
    }

    public getWorkshopStartTime(workshopId: number, groupIndex: number) {
        const workshopIndex = this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.indexOf(workshopId);
        return this.productionItem.productionItemQuantumsGroups[groupIndex].workshopStartTimes[workshopIndex];
    }

    public getViewWorkshopEndTime(workshopId: number, groupIndex: number) {
        const workshopIndex = this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.indexOf(workshopId);
        return this.productionItem.productionItemQuantumsGroups[groupIndex].workshopEndTimes[workshopIndex];
    }
    //#endregion

    //#region Получение времен для отображения значений времен
    public getViewWorkshopDuration(workshopId: number, groupIndex: number) {
        const workshopIndex = this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.indexOf(workshopId);
        return this.order.orderQuantums[this.selectedBlock.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[groupIndex].workshopDurations[workshopIndex];
    }

    public getViewWorkshopStartTime(workshopId: number, groupIndex: number) {
        const workshopIndex = this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.indexOf(workshopId);
        return this.order.orderQuantums[this.selectedBlock.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[groupIndex].workshopStartTimes[workshopIndex];
    }

    public getWorkshopEndTime(workshopId: number, groupIndex: number) {
        const workshopIndex = this.productionItem.productionItemQuantumsGroups[groupIndex].workshopSequence.indexOf(workshopId);
        return this.order.orderQuantums[this.selectedBlock.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[groupIndex].workshopEndTimes[workshopIndex];
    }
    //#endregion

    public getBlockColor(groupIndex: number) {
        return this.colors[groupIndex];
    }

    public selectGroup(groupIndex: number) {
        this.selectedGroup.emit({
            orderQuantumIndex: this.selectedBlock.orderQuantumIndex
            , groupIndex: groupIndex, offset: this.selectedBlock.offset
        });
    }

}
