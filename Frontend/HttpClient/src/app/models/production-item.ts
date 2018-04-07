import { ProductionItemQuantum } from './production-item-quantum';
import { ProductionItemQuantumsGroup } from './production-item-quantums-group';
import { Product } from './product';


export class ProductionItem {
    public id?: number | {};
    public title: string;
    public description: string;
    public productionItemQuantums?: ProductionItemQuantum[];
    public productionItemQuantumsGroups?: ProductionItemQuantumsGroup[];
    public addingItems: Product[] = [];
    public detailsCount?: number;
    public childrenProductionItemsCount?: number;
    public oneItemIncome: number;
}
