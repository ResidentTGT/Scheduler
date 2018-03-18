import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
    MatButtonModule, MatToolbarModule, MatIconModule, MatPaginatorModule, MatListModule, MatProgressSpinnerModule, MatSlideToggleModule,
    MatGridListModule, MatDatepickerModule, MatFormFieldModule, MatNativeDateModule, MatInputModule, MatSliderModule, MatSliderChange,
    MatSelectModule, MatCardModule, MatDialogModule, MAT_DATE_LOCALE, MatRadioModule, MatTableModule, MatPaginatorIntl, MatDividerModule,
    MatMenuModule
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
import { MatPaginatorIntlRu } from './extenders/MatPaginatorIntlRu';
import { HelperService } from './services/helper.service';
import { ParticlesModule } from 'angular-particle';
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';
registerLocaleData(localeRu, 'ru');


const appRoutes: Routes = [
    { path: '', redirectTo: '/', pathMatch: 'full' },
    { path: 'details', component: DetailsComponent },
    { path: 'equipment', component: EquipmentsComponent },

    {
        path: 'production-items',
        children: [
            { path: '', redirectTo: 'view', pathMatch: 'full' },
            { path: 'view', component: ProductionItemsComponent },
            { path: 'create', component: CreateProductionItemComponent }
        ]
    },
    {
        path: 'operations',
        children: [
            { path: '', redirectTo: 'view', pathMatch: 'full' },
            { path: 'view', component: OperationsComponent },
            { path: 'create', component: CreateOperationComponent }
        ]
    },
    {
        path: 'routes',
        children: [
            { path: '', redirectTo: 'view', pathMatch: 'full' },
            { path: 'view', component: RoutesComponent },
            { path: 'create', component: CreateRouteComponent }
        ]
    },
    {
        path: 'orders',
        children: [
            { path: '', redirectTo: 'view', pathMatch: 'full' },
            { path: 'view', component: OrdersComponent },
            { path: 'create', component: CreateOrderComponent },
            { path: 'calculate', component: CalculateOrdersComponent },
        ]
    },

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
        MatDialogModule,
        MatRadioModule,
        MatTableModule,
        MatDividerModule,
        MatMenuModule,
        ParticlesModule
    ],
    providers: [
        BackendApiService,
        HelperService,
        { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' },
        { provide: MatPaginatorIntl, useClass: MatPaginatorIntlRu },
        { provide: LOCALE_ID, useValue: 'ru' },
    ],
    bootstrap: [RootComponent],
})
export class AppModule { }
