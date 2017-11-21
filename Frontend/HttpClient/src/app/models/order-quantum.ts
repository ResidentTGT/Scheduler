import { ProductionItem } from './production-item';

export class OrderQuantum {
    public id?: number | {};
    public productionItem?: ProductionItem;
    public orderId?: number;
    public count: number;
    public itemsCountInOnePart: number;
}
