<script setup>
import { reactive, ref, onMounted, watch } from 'vue';
import { ElMessage } from 'element-plus';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

const goToWarehouse = () => {
  router.push({ name: 'Warehou' });
};



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

// const supplierList = reactive([
//   { id: 1, name: '供应商1' },
//   { id: 2, name: '供应商2' },
//   // 更多供应商
// ]);




const StaffList = reactive([
  { id: 1, name: '采购小明' },
  { id: 2, name: '采购小红' },
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
            await axios.post('/Finance/AP_Detail', yingshou );
        } catch (error) {
            console.log("应付明细错误")
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


// 产品列表，用于后端查询
const productList = ref([]);

// 查询商品信息
const getProductList = async () => {
  try {
    const response = await axios.get('/File_Management/product');
    productList.value = response.data;  // 将商品数据存储在响应式列表中
  } catch (error) {
    ElMessage.error("无法加载商品数据");
  }
};

// 自动填充商品信息
const autoFillProduct = (index, productCodeOrName) => {
  const product = productList.value.find(
    (p) => p.product_code === productCodeOrName || p.name === productCodeOrName
  );
  if (product) {
    // 找到匹配的产品，填充信息
    productTableData[index].productCode = product.product_code;
    productTableData[index].name = product.name;
    productTableData[index].productType = product.product_type;
    productTableData[index].unitPrice = product.unit_price;
  } else {
    ElMessage.warning('未找到匹配的产品');
  }
};

// 监听产品编码或产品名称的变化
watch(
  () => productTableData,
  (newData) => {
    newData.forEach((row, index) => {
      if (row.productCode || row.name) {
        autoFillProduct(index, row.productCode || row.name);
      }
    });
  },
  { deep: true }
);

// 定义供应商列表
const supplierList = reactive([
  // 初始为空或填充默认数据，后续会通过 API 替换
]);

// 获取供应商数据
const fetchSuppliers = async () => {
  try {
    const response = await axios.get('/File_Management/supplier'); // 调用接口
    supplierList.length = 0; // 清空当前数据
    supplierList.push(...response.data); // 用获取的数据填充 supplierList
  } catch (error) {
    console.error('获取供应商数据失败', error);
  }
};


// 初始化订单编号
onMounted(() => {
  purchaseForm.orderId = generateOrderId();
  getProductList();
  fetchSuppliers();

});
</script>

<template>
  <h1>采购单</h1>
  
  <!-- 提交按钮 -->
  <el-form-item>
    <el-button type="primary" @click="submitForm">提交</el-button>
    <el-button @click="resetForm">重置</el-button>
    <el-button @click="goToWarehouse">到货</el-button>
  </el-form-item>
  
  <el-form ref="purchaseFormRef" :model="purchaseForm" :rules="rules" label-width="120px">
    <!-- 采购订单信息 -->
    <el-form-item label="订单编号">
      <el-input v-model="purchaseForm.orderId" disabled></el-input>
    </el-form-item>
    <el-form-item label="采购日期" prop="purchaseDate">
      <el-date-picker v-model="purchaseForm.purchaseDate" type="date"></el-date-picker>
    </el-form-item>
    <el-form-item label="工作人员" prop="staff">
      <el-select v-model="purchaseForm.staff" placeholder="请选择工作人员">
        <el-option v-for="supplier in StaffList" :key="supplier.id" :label="supplier.name" :value="supplier.id" />
      </el-select>
    </el-form-item>

    <el-form-item label="支付方式" prop="paymentMethod">
      <el-select v-model="purchaseForm.paymentMethod">
        <el-option label="支付宝" value="支付宝"></el-option>
        <el-option label="微信" value="微信"></el-option>
        <el-option label="银行卡" value="银行卡"></el-option>
        <el-option label="货到付款" value="货到付款"></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="结算方式" prop="settlementMethod">
      <el-select v-model="purchaseForm.settlementMethod">
        <el-option label="月结" value="月结"></el-option>
        <el-option label="季结" value="季结"></el-option>
        <el-option label="货到付款" value="货到付款"></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="币种" prop="currency">
      <el-select v-model="purchaseForm.currency">
        <el-option label="人民币" value="人民币"></el-option>
        <el-option label="美元" value="美元"></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="供应商" prop="supplierDeliveryMethodId">
      <el-select v-model="purchaseForm.supplierDeliveryMethodId" placeholder="请选择供应商">
        <el-option v-for="supplier in supplierList" :key="supplier.id" :label="supplier.name" :value="supplier.id" />
      </el-select>
    </el-form-item>

    <!-- 产品信息列表 -->
    <el-table :data="productTableData" style="width: 100%" border>
      <el-table-column label="产品编码">
        <template #default="scope">
          <el-input v-model="scope.row.productCode" placeholder="输入产品编码" />
        </template>
      </el-table-column>
      <el-table-column label="产品名称">
        <template #default="scope">
          <el-input v-model="scope.row.name" placeholder="输入产品名称" />
        </template>
      </el-table-column>
      <el-table-column label="产品类型">
        <template #default="scope">
          <el-input v-model="scope.row.productType" placeholder="输入产品类型" disabled />
        </template>
      </el-table-column>
      <el-table-column label="单价">
        <template #default="scope">
          <el-input-number v-model="scope.row.unitPrice" :min="0" disabled />
        </template>
      </el-table-column>
      <el-table-column label="数量">
        <template #default="scope">
          <el-input-number v-model="scope.row.count" :min="0" />
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
