import { ProductionItemQuantum } from './production-item-quantum';
import { ProductionItemQuantumsGroup } from './production-item-quantums-group';

export class ProductionItem {
    public id?: number | {};
    public title: string;
    public description: string;
    public parentProductionItemId: number;
    public parentProductionItemTitle: string;
    public isNode: Boolean;
    public productionItemQuantums?: ProductionItemQuantum[];
    public productionItemQuantumsGroups?: ProductionItemQuantumsGroup[];
}
