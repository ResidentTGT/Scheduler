import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    MatButtonModule, MatToolbarModule, MatIconModule, MatPaginatorModule, MatListModule, MatProgressSpinnerModule, MatSlideToggleModule,
    MatGridListModule, MatDatepickerModule, MatFormFieldModule, MatNativeDateModule, MatInputModule, MatSliderModule, MatSliderChange,
    MatSelectModule, MatCardModule, MatDialogModule, MAT_DATE_LOCALE
} from '@angular/material';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RootComponent } from './components/root/root.component';
import { HeaderComponent } from './components/header/header.component';
import { DetailsComponent } from './components/details/details.component';
import { BackendApiService } from './services/backend-api.service';
import { EquipmentsComponent } from './components/equipments/equipments.component';
import { EquipmentTypePipe } from './pipes/equipment-type.pipe';
import { OrdersComponent } from './components/orders/orders.component';
import { ProductionItemsComponent } from './components/production-items/production-items.component';
import { CreateProductionItemComponent } from './components/create-production-item/create-production-item.component';
import { OperationsComponent } from './components/operations/operations.component';
import { CreateOperationComponent } from './components/create-operation/create-operation.component';
import { OperationTypePipe } from './pipes/operation-type.pipe';
import { CreateOrderComponent } from './components/create-order/create-order.component';
import { OrderStatePipe } from './pipes/order-state.pipe';
import { RoutesComponent } from './components/routes/routes.component';
import { CreateRouteComponent } from './components/create-route/create-route.component';
import { CalculateOrdersComponent } from './components/calculate-orders/calculate-orders.component';
import { ViewOrderQuantumsComponent } from './components/view-calculate-results/view-order-quantums/view-order-quantums.component';
import { ViewProductionItemQuantumsGroupsComponent } from './components/view-calculate-results/view-production-item-quantums-groups/view-production-item-quantums-groups.component';
import { ViewProductionItemQuantumGroupComponent } from './components/view-calculate-results/view-production-item-quantum-group/view-production-item-quantum-group.component';

const appRoutes: Routes = [
    { path: '', redirectTo: '/', pathMatch: 'full' },
    { path: 'details', component: DetailsComponent },
    { path: 'equipment', component: EquipmentsComponent },
    { path: 'orders', component: OrdersComponent },
    { path: 'production-items', component: ProductionItemsComponent },
    { path: 'operations', component: OperationsComponent },
    { path: 'routes', component: RoutesComponent },
    { path: 'calculate-orders', component: CalculateOrdersComponent },
    // { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    declarations: [
        RootComponent,
        HeaderComponent,
        DetailsComponent,
        EquipmentsComponent,
        EquipmentTypePipe,
        OrdersComponent,
        ProductionItemsComponent,
        CreateProductionItemComponent,
        OperationsComponent,
        CreateOperationComponent,
        OperationTypePipe,
        CreateOrderComponent,
        OrderStatePipe,
        RoutesComponent,
        CreateRouteComponent,
        CalculateOrdersComponent,
        ViewOrderQuantumsComponent,
        ViewProductionItemQuantumsGroupsComponent,
        ViewProductionItemQuantumGroupComponent
    ],
    imports: [
        RouterModule.forRoot(appRoutes),
        BrowserModule,
        HttpModule,
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatPaginatorModule,
        BrowserAnimationsModule,
        MatListModule,
        MatProgressSpinnerModule,
        MatGridListModule,
        MatDatepickerModule,
        MatFormFieldModule,
        MatNativeDateModule,
        MatInputModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSelectModule,
        FormsModule,
        MatCardModule,
        MatDialogModule
    ],
    providers: [BackendApiService, { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' }],
    bootstrap: [RootComponent],
    entryComponents: [
        CreateProductionItemComponent,
        CreateOperationComponent,
        CreateOrderComponent,
        CreateRouteComponent
    ]
})
export class AppModule { }
