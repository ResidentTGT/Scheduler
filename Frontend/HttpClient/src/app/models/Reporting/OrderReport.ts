import { OrderBlock } from './OrderBlock';
import { Order } from '../order';

export class OrderReport {
    public id: number;

    public creationTime: Date;

    public orderBlocks: OrderBlock[];

    public order: Order;

    static fromJSON(obj: any) {
        if (!obj) {
            return null;
        }

        return Object.assign(
            new OrderReport(),
            obj,
            {
                orderBlocks: obj.orderBlocks ? obj.orderBlocks.map(g => OrderBlock.fromJSON(g)) : []
            }
        );
    }
}
