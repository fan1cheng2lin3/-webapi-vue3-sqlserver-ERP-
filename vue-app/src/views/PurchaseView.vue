<template>
    <el-form 
      ref="purchaseFormRef" 
      :model="purchaseForm" 
      :rules="rules" 
      label-width="120px">
      <el-form-item label="订单编号" prop="order_id">
        <el-input v-model="purchaseForm.order_id" />
      </el-form-item>
      <el-form-item label="采购日期" prop="purchase_date">
        <el-date-picker v-model="purchaseForm.purchase_date" type="date" placeholder="选择日期" />
      </el-form-item>
      <el-form-item label="工作人员" prop="staff">
        <el-input v-model="purchaseForm.staff" />
      </el-form-item>
      <el-form-item label="支付方式" prop="payment_method">
        <el-input v-model="purchaseForm.payment_method" />
      </el-form-item>
      <el-form-item label="结算方式" prop="settlement_method">
        <el-input v-model="purchaseForm.settlement_method" />
    </el-form-item>
    <el-form-item label="币种" prop="currency">
      <el-select v-model="purchaseForm.currency" placeholder="选择币种">
        <el-option label="人民币" value="人民币"></el-option>
        <el-option label="美元" value="美元"></el-option>
      </el-select>
    </el-form-item>
      <el-form-item label="供应商" prop="supplier_delivery_method_id">
        <el-input v-model="purchaseForm.supplier_delivery_method_id" />
      </el-form-item>
      <el-form-item label="数量" prop="cout">
        <el-input-number v-model="purchaseForm.cout" />
      </el-form-item>
      <el-form-item label="单价" prop="unit_price">
        <el-input-number v-model="purchaseForm.unit_price" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm">审核</el-button>
        <el-button @click="resetForm">重置</el-button>
      </el-form-item>
    </el-form>
  </template>
  
  <script setup>
  import { reactive, ref } from 'vue';
  import { ElMessage } from 'element-plus';
  import axios from 'axios';
  
// 表单数据
const purchaseForm = reactive({
  order_id: '',
  purchase_date: '',
  staff: '',
  payment_method: '',
  settlement_method: '',
  currency: '人民币', // 默认值为人民币
  supplier_delivery_method_id: '',
  cout: 0,
  unit_price: 0
});
  
  // 表单引用
  const purchaseFormRef = ref(null);
  
  // 表单校验规则
  const rules = reactive({
    order_id: [{ required: true, message: '订单编号不能为空', trigger: 'blur' }],
    purchase_date: [{ required: true, message: '采购日期不能为空', trigger: 'change' }],
    staff: [{ required: true, message: '工作人员不能为空', trigger: 'blur' }],
    payment_method: [{ required: true, message: '支付方式不能为空', trigger: 'blur' }],
    settlement_method: [{ required: true, message: '结算方式不能为空', trigger: 'blur' }],
    currency: [{ required: true, message: '币种不能为空', trigger: 'change' }], // 币种校验规则
    supplier_delivery_method_id: [{ required: true, message: '供应商不能为空', trigger: 'blur' }],
    cout: [
      { required: true, message: '数量不能为空', trigger: 'blur' },
      { type: 'number', message: '数量必须是数字', trigger: 'blur' }
    ],
    unit_price: [
      { required: true, message: '单价不能为空', trigger: 'blur' },
      { type: 'number', message: '单价必须是数字', trigger: 'blur' }
    ]
  });
  
  // 提交表单
  const submitForm = async () => {
    purchaseFormRef.value.validate(async (valid) => {
      if (valid) {
        try {
          await axios.post('/Purchase/Addpurchase', purchaseForm);
   
          ElMessage.success('添加成功');
          resetForm(); // 清空表单
        } catch (error) {
          console.error(error);
          ElMessage.error('添加失败');
        }
      } else {
        ElMessage.warning('请完成所有必填字段');
      }
    });





    
  };
  
  // 重置表单
  const resetForm = () => {
    if (purchaseFormRef.value) {
      purchaseFormRef.value.resetFields();
    }
  };
  </script>
  
  <style scoped>
  /* 你可以在这里添加一些样式 */
  </style>
  