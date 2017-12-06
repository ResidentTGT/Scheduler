import { Component, OnInit, ViewEncapsulation, Input, OnChanges } from '@angular/core';
import { Order } from '../../../models/order';
import { ProductionItemQuantumsGroup } from '../../../models/production-item-quantums-group';
import { ProductionItemQuantum } from '../../../models/production-item-quantum';
import { Detail } from '../../../models/detail';
import { randomColor } from 'randomcolor';

@Component({
    selector: 'sch-view-production-item-quantum-group',
    templateUrl: './view-production-item-quantum-group.component.html',
    styleUrls: ['./view-production-item-quantum-group.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewProductionItemQuantumGroupComponent implements OnInit, OnChanges {

    @Input()
    public order: Order;
    @Input()
    public selectedGroup: { orderQuantumIndex: number, groupIndex: number, offset: number };

    public colors: any[] = [];
    public group: ProductionItemQuantumsGroup;
    public equipments: Array<{ id: number, name: string }> = new Array<{ id: number, name: string }>();

    constructor() { }

    ngOnInit() {
        this.defaultZoom();

        this.generateColors();
        this.setAllEquipments();
    }

    ngOnChanges() {
        this.defaultZoom();

        this.generateColors();
        this.setAllEquipments();
    }

    public defaultZoom() {
        this.group = new ProductionItemQuantumsGroup();
        this.group.productionItemQuantums = new Array<ProductionItemQuantum>();
        this.order.orderQuantums[this.selectedGroup.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[this.selectedGroup.groupIndex].productionItemQuantums
            .forEach(iq => this.group.productionItemQuantums.push({
                machiningDurations: Object.assign(new Array<number>(), iq.machiningDurations),
                startTimes: Object.assign(new Array<number>(), iq.startTimes),
                endTimes: Object.assign(new Array<number>(), iq.endTimes),
                count: iq.count,
                detail: Object.assign(new Detail(), iq.detail)
            }));
    }

    private generateColors() {
        this.group.productionItemQuantums.forEach(q => this.colors.push(randomColor({ luminosity: 'light' })));
    }

    public setAllEquipments() {
        const equipments = new Array<{ id: number, name: string }>();
        this.group.productionItemQuantums.forEach(g => g.detail.equipmentsIdSequence.forEach(id => {
            if (!equipments.some(e => e.id === id)) {
                equipments.push({ id: id, name: g.detail.equipmentsNameSequence[g.detail.equipmentsIdSequence.indexOf(id)] });
            }
        }
        ));
        this.equipments = equipments;
    }

    public isProductionItemQuantumInEquipment(equipment: any, productionItemQuantumIndex: number) {
        return this.group.productionItemQuantums[productionItemQuantumIndex].detail.equipmentsIdSequence.some(s => s === equipment.id);
    }

    public getMachiningDuration(equipment: any, productionItemQuantumIndex: number) {
        const equipmentIndex = this.group.productionItemQuantums[productionItemQuantumIndex]
            .detail.equipmentsIdSequence.indexOf(equipment.id);
        return this.group.productionItemQuantums[productionItemQuantumIndex].machiningDurations[equipmentIndex];
    }

    public getMachiningStartTime(equipment: any, productionItemQuantumIndex: number) {
        const equipmentIndex = this.group.productionItemQuantums[productionItemQuantumIndex]
            .detail.equipmentsIdSequence.indexOf(equipment.id);
        return this.group.productionItemQuantums[productionItemQuantumIndex].startTimes[equipmentIndex];
    }

    public getBlockColor(productionItemQuantumIndex: number) {
        return this.colors[productionItemQuantumIndex];
    }

    public zoomPlus() {
        this.group.productionItemQuantums.forEach(g => {
            for (let i = 0; i < g.machiningDurations.length; i++) {
                g.machiningDurations[i] *= 1.1;
                g.startTimes[i] *= 1.1;
            }
        });
    }

    public zoomMinus() {
        this.group.productionItemQuantums.forEach(g => {
            for (let i = 0; i < g.machiningDurations.length; i++) {
                g.machiningDurations[i] *= 0.9;
                g.startTimes[i] *= 0.9;
            }
        });
    }

    //#region Получение времен для отображения значений времен
    public getViewMachiningDuration(equipment: any, productionItemQuantumIndex: number) {
        const equipmentIndex = this.group.productionItemQuantums[productionItemQuantumIndex]
            .detail.equipmentsIdSequence.indexOf(equipment.id);
        return this.order.orderQuantums[this.selectedGroup.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[this.selectedGroup.groupIndex]
            .productionItemQuantums[productionItemQuantumIndex]
            .machiningDurations[equipmentIndex];
    }

    public getViewMachiningStartTime(equipment: any, productionItemQuantumIndex: number) {
        const equipmentIndex = this.group.productionItemQuantums[productionItemQuantumIndex]
            .detail.equipmentsIdSequence.indexOf(equipment.id);
        return this.order.orderQuantums[this.selectedGroup.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[this.selectedGroup.groupIndex]
            .productionItemQuantums[productionItemQuantumIndex]
            .startTimes[equipmentIndex];
    }

    public getViewMachiningEndTime(equipment: any, productionItemQuantumIndex: number) {
        const equipmentIndex = this.group.productionItemQuantums[productionItemQuantumIndex]
            .detail.equipmentsIdSequence.indexOf(equipment.id);
        return this.order.orderQuantums[this.selectedGroup.orderQuantumIndex]
            .productionItem.productionItemQuantumsGroups[this.selectedGroup.groupIndex]
            .productionItemQuantums[productionItemQuantumIndex]
            .endTimes[equipmentIndex];
    }
    //#endregion

}
