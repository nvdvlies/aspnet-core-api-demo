import { TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { CustomerDto } from '@api/api.generated.clients';
import { CustomerUpdatedEvent } from '@api/signalr.generated.services';
import { CustomerDomainEntityService } from '@domain/customer/customer-domain-entity.service';
import { CustomerStoreService } from '@domain/customer/customer-store.service';
import { of, switchMap, tap } from 'rxjs';

function spyPropertyGetter<T, K extends keyof T>(
  spyObj: jasmine.SpyObj<T>,
  propName: K
): jasmine.Spy<() => T[K]> {
  return Object.getOwnPropertyDescriptor(spyObj, propName)?.get as jasmine.Spy<() => T[K]>;
}

interface TestSetupOptions {
  parameterName: string;
  parameterValue: string;
}

describe('CustomerDomainEntityService', () => {
  let subject: CustomerDomainEntityService;
  let storeServiceSpy: jasmine.SpyObj<CustomerStoreService>;

  function init(options?: TestSetupOptions): void {
    var paramMap = {} as any;
    paramMap[options?.parameterName ?? 'id'] = options?.parameterValue ?? '123';

    storeServiceSpy = jasmine.createSpyObj(
      'CustomerStoreService',
      ['getById', 'create', 'update', 'delete'],
      ['customerUpdatedInStore$']
    ) as jasmine.SpyObj<CustomerStoreService>;
    TestBed.configureTestingModule({
      providers: [
        CustomerDomainEntityService,
        FormBuilder,
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: convertToParamMap(paramMap) } }
        },
        { provide: CustomerStoreService, useValue: storeServiceSpy }
      ]
    });

    spyPropertyGetter(storeServiceSpy, 'customerUpdatedInStore$').and.returnValue(
      of<[CustomerUpdatedEvent, CustomerDto]>([{} as CustomerUpdatedEvent, new CustomerDto()])
    );

    subject = TestBed.inject(CustomerDomainEntityService);
  }

  describe('initFromRoute', () => {
    describe("when id param in route is 'new'", () => {
      beforeEach(() => {
        init({ parameterValue: 'new' } as TestSetupOptions);
      });

      it('should not call store.getById', (done) => {
        // Arrange

        // Act
        subject.initFromRoute().subscribe((_) => {
          // Assert
          expect(storeServiceSpy.getById).not.toHaveBeenCalledTimes(1);
          expect(subject.form.controls.id.value).toBeNull();
          done();
        });
      });
    });

    describe("when id param in route is '123'", () => {
      beforeEach(() => {
        init();
      });

      it('should call store.getById', (done) => {
        // Arrange
        const customerId = '123';
        const customer = new CustomerDto();
        customer.id = customerId;
        customer.name = 'test';
        storeServiceSpy.getById.and.returnValue(of(customer));

        // Act
        subject.initFromRoute().subscribe((_) => {
          // Assert
          expect(storeServiceSpy.getById).toHaveBeenCalledOnceWith(customerId);
          expect(subject.form.controls.id.value).toBe(customer.id);
          expect(subject.form.controls.name!.value).toBe(customer.name);
          done();
        });
      });
    });
  });

  describe('create', () => {
    beforeEach(() => {
      init();
    });

    it('should call store.create', (done) => {
      // Arrange
      const customerName = 'Microsoft';
      storeServiceSpy.create.and.callFake(function (customer) {
        return of(customer);
      });

      // Act
      subject
        .new()
        .pipe(
          tap(() => {
            subject.form.controls.name!.setValue(customerName);
          }),
          switchMap(() => subject.create())
        )
        .subscribe((_) => {
          // Assert
          expect(storeServiceSpy.create).toHaveBeenCalledOnceWith(
            jasmine.objectContaining({ name: customerName })
          );
          done();
        });
    });
  });

  describe('update', () => {
    beforeEach(() => {
      init();
    });

    it('should call store.update', (done) => {
      // Arrange
      const customerId = '123';
      const customer = new CustomerDto();
      customer.id = customerId;
      customer.name = 'Microsoft';
      const newCustomerName = 'Google';

      storeServiceSpy.getById.and.returnValue(of(customer));
      storeServiceSpy.update.and.callFake(function (customer) {
        return of(customer);
      });

      // Act
      subject
        .getById(customerId)
        .pipe(
          tap(() => {
            subject.form.controls.name!.setValue(newCustomerName);
          }),
          switchMap(() => subject.update())
        )
        .subscribe((_) => {
          // Assert
          expect(storeServiceSpy.update).toHaveBeenCalledOnceWith(
            jasmine.objectContaining({ id: customerId, name: newCustomerName })
          );
          done();
        });
    });
  });

  describe('upsert', () => {
    beforeEach(() => {
      init();
    });

    it('when dealing with a new entity it should call store.create', (done) => {
      // Arrange
      const customerName = 'Microsoft';
      storeServiceSpy.create.and.callFake(function (customer) {
        return of(customer);
      });

      // Act
      subject
        .new()
        .pipe(
          tap(() => {
            subject.form.controls.name!.setValue(customerName);
          }),
          switchMap(() => subject.upsert())
        )
        .subscribe((_) => {
          // Assert
          expect(storeServiceSpy.create).toHaveBeenCalledOnceWith(
            jasmine.objectContaining({ name: customerName })
          );
          done();
        });
    });

    it('when dealing with an existing entity it should call store.update', (done) => {
      // Arrange
      const customerId = '123';
      const customer = new CustomerDto();
      customer.id = customerId;
      customer.name = 'Microsoft';
      const newCustomerName = 'Google';

      storeServiceSpy.getById.and.returnValue(of(customer));
      storeServiceSpy.update.and.callFake(function (customer) {
        return of(customer);
      });

      // Act
      subject
        .getById(customerId)
        .pipe(
          tap(() => {
            subject.form.controls.name!.setValue(newCustomerName);
          }),
          switchMap(() => subject.upsert())
        )
        .subscribe((_) => {
          // Assert
          expect(storeServiceSpy.update).toHaveBeenCalledOnceWith(
            jasmine.objectContaining({ id: customerId, name: newCustomerName })
          );
          done();
        });
    });
  });

  describe('delete', () => {
    beforeEach(() => {
      init();
    });

    it('should call store.delete', (done) => {
      // Arrange
      const customerId = '123';
      const customer = new CustomerDto();
      customer.id = customerId;

      storeServiceSpy.getById.and.returnValue(of(customer));
      storeServiceSpy.delete.and.returnValue(of(undefined));

      // Act
      subject
        .getById(customerId)
        .pipe(switchMap(() => subject.delete()))
        .subscribe((_) => {
          // Assert
          expect(storeServiceSpy.delete).toHaveBeenCalledOnceWith(customerId);
          done();
        });
    });
  });
});
