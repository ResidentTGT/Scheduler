import { Component, OnInit, ViewEncapsulation, Input, OnChanges } from '@angular/core';
import { Order } from '../../../models/order';
import { ProductionItemQuantumsGroup } from '../../../models/production-item-quantums-group';
import { ProductionItemQuantum } from '../../../models/production-item-quantum';
import { Detail } from '../../../models/detail';
import { randomColor } from 'randomcolor';
import { GroupBlock } from '../../../models/Reporting/GroupBlock';
import { DetailsBatchBlock } from '../../../models/Reporting/DetailsBatchBlock';
import { Equipment } from '../../../models/equipment';

@Component({
    selector: 'sch-view-production-item-quantum-group',
    templateUrl: './view-production-item-quantum-group.component.html',
    styleUrls: ['./view-production-item-quantum-group.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewProductionItemQuantumGroupComponent implements OnInit {

    @Input()
    public set setSelectedGroup(block: GroupBlock) {
        this.selectedGroup = block;
        this.defaultZoom();

        this.generateColors();
        this.setAllEquipments();
    }

    public colors: { detailId: number, color: string }[] = [];
    public selectedGroup: GroupBlock;
    public filteredDetailsBlocks: DetailsBatchBlock[] = [];
    public allEquipments: Equipment[] = [];

    constructor() { }

    ngOnInit() {
    }

    public defaultZoom() {
        this.filteredDetailsBlocks = new Array<DetailsBatchBlock>();
        const minTime = this.selectedGroup.detailsBatchBlocks.map(b => b.startTime).sort(function (a, b) { return a - b; })[0];
        this.selectedGroup.detailsBatchBlocks.forEach(g => {
            this.filteredDetailsBlocks.push(Object.assign(new DetailsBatchBlock(), g));
        });
        this.filteredDetailsBlocks.forEach(b => b.startTime -= minTime);
    }

    private generateColors() {
        const set = new Set<number>();
        this.selectedGroup.detailsBatchBlocks.forEach(g => set.add(g.detailId));
        set.forEach(id => this.colors.push({ detailId: id, color: randomColor({ luminosity: 'light' }) }));
    }

    public setAllEquipments() {
        this.selectedGroup.detailsBatchBlocks.forEach(g => {
            if (!this.allEquipments.some(w => w.id === g.equipment.id)) {
                this.allEquipments.push(g.equipment);
            }
        });
    }

    public getBlockColor(id: number) {
        return this.colors.filter(c => c.detailId === id)[0].color;
    }

    public GetRealDetailsBlock(id: number): DetailsBatchBlock {
        return this.selectedGroup.detailsBatchBlocks.filter(b => b.id === id)[0];
    }

    public getBlocksInEquipment(equipment: Equipment) {
        return this.filteredDetailsBlocks.filter(g => g.equipment.id === equipment.id);
    }

    public zoomPlus() {
        this.filteredDetailsBlocks.forEach(b => {
            b.duration *= 1.1;
            b.startTime *= 1.1;
        });
    }

    public zoomMinus() {
        this.filteredDetailsBlocks.forEach(b => {
            b.duration *= 0.9;
            b.startTime *= 0.9;
        });
    }

}
