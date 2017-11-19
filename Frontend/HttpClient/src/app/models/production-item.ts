import { ProductionItemQuantum } from './production-item-quantum';

export class ProductionItem {
    public id?: number | {};
    public title: string;
    public description: string;
    public parentProductionItemId: number;
    public parentProductionItemTitle: string;
    public isNode: Boolean;
    public productionItemQuantums?: ProductionItemQuantum[];
}
