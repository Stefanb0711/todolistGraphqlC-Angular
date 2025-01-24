import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodoInputFieldComponent } from './todo-input-field.component';

describe('TodoInputFieldComponent', () => {
  let component: TodoInputFieldComponent;
  let fixture: ComponentFixture<TodoInputFieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TodoInputFieldComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodoInputFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
