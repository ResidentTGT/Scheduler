import { OrderQuantum } from './order-quantum';

export class Order {
    public id: number;
    public name: string;
    public description: string;
    public state: OrderState;
    public plannedBeginDate: Date;
    public plannedEndDate: Date;
    public orderQuantums: OrderQuantum[];
}
export enum OrderState {
    Undefined = 0,
    Ready = 1,
    InProcess = 2
}
