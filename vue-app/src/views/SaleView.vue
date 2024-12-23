<script setup>
import { reactive, ref, onMounted } from 'vue';
import { ElMessage } from 'element-plus';
import axios from 'axios';

// 自动生成订单编号
const generateOrderId = () => {
  const now = new Date();
  return (
    now.getMinutes().toString().padStart(2, '0') +
    now.getSeconds().toString().padStart(2, '0') +
    now.getMilliseconds().toString().padStart(3, '0')
  );
};

// 表单数据：采购订单信息
const purchaseForm = reactive({
  orderId: '', // 自动生成的订单编号
  purchaseDate: '',
  staff: '',
  paymentMethod: '微信',
  settlementMethod: '月结',
  currency: '人民币',
  supplierDeliveryMethodId: '',
});



// 初始化产品信息
const initProduct = () => ({
  name: '',
  productCode: '',
  productType: '',
  supplierName: '',
  unitPrice: 0,
  count: 0,
});

const productTableData = reactive([initProduct()]);

const supplierList = reactive([
  { id: 1, name: '供应商1' },
  { id: 2, name: '供应商2' },
  // 更多供应商
]);


const StaffList = reactive([
  { id: 1, name: '小王' },
  { id: 2, name: '小李' },
  // 更多供应商
]);

// 表单引用
const purchaseFormRef = ref(null);

// 校验规则
const rules = reactive({
  purchaseDate: [{ required: true, message: '采购日期不能为空', trigger: 'change' }],
  staff: [{ required: true, message: '工作人员不能为空', trigger: 'blur' }],
  paymentMethod: [{ required: true, message: '支付方式不能为空', trigger: 'blur' }],
  settlementMethod: [{ required: true, message: '结算方式不能为空', trigger: 'blur' }],
  currency: [{ required: true, message: '币种不能为空', trigger: 'change' }],
  supplierDeliveryMethodId: [{ required: true, message: '供应商不能为空', trigger: 'blur' }],
});

// 添加新产品行
const addNewProduct = () => {
  productTableData.push(initProduct());
};

// 删除产品行
const deleteProduct = (index) => {
  productTableData.splice(index, 1);
};


const yingshouForm = reactive({
  detail_id: 1, // 自动生成的订单编号
  receivables: '',
  invoice_number: '',
  amount: 9,
  due_date: '',
  created_at: '',
});

const calculateAmount = (products) => {
  return products.reduce((total, product) => {
    return total + (product.unitPrice * product.count);
  }, 0);
};

const calculateDueDate = (settlementMethod, purchaseDate) => {
  const purchaseDateObj = new Date(purchaseDate);

  if (settlementMethod === '月结') {
    purchaseDateObj.setMonth(purchaseDateObj.getMonth() + 1); // 月结：加一个月
  } else if (settlementMethod === '季结') {
    purchaseDateObj.setMonth(purchaseDateObj.getMonth() + 3); // 季结：加三个月
  } else if (settlementMethod === '货到付款') {
    return purchaseDateObj.toISOString(); // 货到付款使用采购日期
  }

  return purchaseDateObj.toISOString(); // 默认返回修改后的到期日期
};

// 提交表单
const submitForm = async () => {
  purchaseFormRef.value.validate(async (valid) => {
    if (valid) {
      // 构建 payload
      const payload = {
        ...purchaseForm,
        purchaseDate: purchaseForm.purchaseDate ? new Date(purchaseForm.purchaseDate).toISOString() : '',
        products: productTableData.map((product) => ({
        name: product.name,
        productCode: product.productCode,
        productType: product.productType,
        supplierName: product.supplierName,
        unitPrice: parseFloat(product.unitPrice), // 确保单价是浮点数
        count: parseInt(product.count, 10), // 确保数量是整数
        })),
      };
      const yingshou = {
  ...yingshouForm,
  receivables: supplierList.find(supplier => supplier.id === purchaseForm.supplierDeliveryMethodId)?.name || "默认供应商", // 动态替换为供应商名称
  amount: calculateAmount(productTableData), // 自动计算总价
  due_date: calculateDueDate(purchaseForm.settlementMethod, purchaseForm.purchaseDate), // 动态计算到期日期
};
      try {
        await axios.post('/Purchase/Addpurchase',payload );
        ElMessage.success('添加成功');
        resetForm();

      //提交应收
         try {
            await axios.post('/Finance/AR_Detail', yingshou );
        } catch (error) {
            console.log("应收明细错误")
        }

      } catch (error) {
        const message = error.response?.data?.message || '网络或服务端错误';
        ElMessage.error(`添加失败：${message}`);
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
  purchaseForm.orderId = generateOrderId(); // 调用 generateOrderId() 来生成新的订单编号
  productTableData.splice(0);
  addNewProduct(); // 添加初始产品行
};


// 初始化订单编号
onMounted(() => {
  purchaseForm.orderId = generateOrderId();
});
</script>

<template>
    <h1>销售表</h1>
   <!-- 提交按钮 -->
   <el-form-item>
      <el-button type="primary" @click="submitForm">审核</el-button>
      <el-button @click="resetForm">重置</el-button>
      <el-button @click="resetForm">发货通知</el-button>
    </el-form-item>
  <el-form ref="purchaseFormRef" :model="purchaseForm" :rules="rules" label-width="120px">
    <!-- 采购订单信息 -->
    <el-form-item label="销售编号">
      <el-input v-model="purchaseForm.orderId" disabled></el-input>
    </el-form-item>
    <el-form-item label="联系人">
      <el-input v-model="purchaseForm.orderId" disabled></el-input>
    </el-form-item>
    <el-form-item label="联系电话">
      <el-input v-model="purchaseForm.orderId" disabled></el-input>
    </el-form-item>

    <el-form-item label="下单日期" prop="purchaseDate">
      <el-date-picker v-model="purchaseForm.purchaseDate" type="date"></el-date-picker>
    </el-form-item>

    <el-form-item label="单位名称" prop="supplierDeliveryMethodId">
      <el-select v-model="purchaseForm.supplierDeliveryMethodId" placeholder="请选择供应商">
        <el-option v-for="supplier in supplierList" :key="supplier.id" :label="supplier.name" :value="supplier.id" />
      </el-select>
    </el-form-item>
    <el-form-item label="单位地址" prop="staff">
      <el-select v-model="purchaseForm.staff" placeholder="请选择工作人员">
        <el-option v-for="supplier in StaffList" :key="supplier.id" :label="supplier.name" :value="supplier.id" />
      </el-select>
      
    </el-form-item>
 

    <!-- 产品信息列表 -->
    
    <el-table :data="productTableData" style="width: 100%" border>
      <el-table-column label="产品编码">
        <template #default="scope">
          <el-input v-model="scope.row.productCode" placeholder="输入产品编码"></el-input>
        </template>
      </el-table-column>
      <el-table-column label="产品名称">
        <template #default="scope">
          <el-input v-model="scope.row.name" placeholder="输入产品名称"></el-input>
        </template>
      </el-table-column>
     
      
      <el-table-column label="单价">
        <template #default="scope">
          <el-input-number v-model="scope.row.unitPrice" :min="0"></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="数量">
        <template #default="scope">
          <el-input-number v-model="scope.row.count" :min="0"></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="操作">
        <template #default="scope">
          <el-button @click="deleteProduct(scope.$index)" type="danger">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-button type="primary" @click="addNewProduct">新增产品</el-button>

   
  </el-form>
</template>
