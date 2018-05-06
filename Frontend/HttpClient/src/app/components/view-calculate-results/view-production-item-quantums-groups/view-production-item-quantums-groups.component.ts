import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnChanges } from '@angular/core';
import { Order } from '../../../models/order';
import { ProductionItem } from '../../../models/production-item';
import { randomColor } from 'randomcolor';
import { ProductionItemQuantumsGroup } from '../../../models/production-item-quantums-group';
import { OrderBlock } from '../../../models/Reporting/OrderBlock';
import { GroupBlock } from '../../../models/Reporting/GroupBlock';
import { Workshop } from '../../../models/workshop';

@Component({
    selector: 'sch-view-production-item-quantums-groups',
    templateUrl: './view-production-item-quantums-groups.component.html',
    styleUrls: ['./view-production-item-quantums-groups.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewProductionItemQuantumsGroupsComponent implements OnInit {

    @Input()
    public set setSelectedBlock(block: OrderBlock) {
        this.selectedBlock = block;
        this.defaultZoom();

        this.generateColors();
        this.setAllWorkshops();
    }

    @Output()
    selectedGroup: EventEmitter<GroupBlock> = new EventEmitter<GroupBlock>();

    public filteredGroupBlocks: GroupBlock[] = [];
    public selectedBlock: OrderBlock;
    public colors: { groupIndex: number, color: string }[] = [];
    public allWorkshops: Workshop[] = [];

    constructor() { }

    ngOnInit() {
    }

    private generateColors() {
        const set = new Set<number>();
        this.selectedBlock.groupBlocks.forEach(g => set.add(g.groupIndex));
        set.forEach(index => this.colors.push({ groupIndex: index, color: randomColor({ luminosity: 'light' }) }));
    }

    public defaultZoom() {
        this.filteredGroupBlocks = new Array<GroupBlock>();
        const minTime = this.selectedBlock.groupBlocks.map(b => b.startTime).sort(function (a, b) { return a - b; })[0];
        this.selectedBlock.groupBlocks.forEach(g => {
            this.filteredGroupBlocks.push(Object.assign(new GroupBlock(), g));
        });
        this.filteredGroupBlocks.forEach(b => b.startTime -= minTime);
    }

    public getGroupsInWorkshop(workshop: Workshop) {
        return this.filteredGroupBlocks.filter(g => g.workshop.id === workshop.id);
    }

    public GetRealGroupBlock(id: number): GroupBlock {
        return this.selectedBlock.groupBlocks.filter(b => b.id === id)[0];
    }

    public zoomPlus() {
        this.filteredGroupBlocks.forEach(b => {
            b.duration *= 1.1;
            b.startTime *= 1.1;
        });
    }

    public zoomMinus() {
        this.filteredGroupBlocks.forEach(b => {
            b.duration *= 0.9;
            b.startTime *= 0.9;
        });
    }

    public setAllWorkshops() {
        this.selectedBlock.groupBlocks.forEach(g => {
            if (!this.allWorkshops.some(w => w.id === g.workshop.id)) {
                this.allWorkshops.push(g.workshop);
            }
        });
    }

    public getBlockColor(id: number) {
        return this.colors.filter(c => c.groupIndex === id)[0].color;
    }

    public selectGroup(groupBlock: GroupBlock) {
        this.selectedGroup.emit(Object.assign(new GroupBlock(), groupBlock));
    }

}
